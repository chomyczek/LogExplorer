// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using LogExplorer.Models;
using LogExplorer.Services.Core;
using LogExplorer.Services.Extensions;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;
using LogExplorer.Services.OutputSystem;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

#endregion

namespace LogExplorer.ViewModels
{
	public class RootViewModel : MvxViewModel
	{
		#region Fields

		private readonly IExplorer explorer;

		private readonly Logger logger;

		private readonly IManager manager;

		private readonly ITester tester;

		private CancellationTokenSource cts;

		private bool isRerunAvailable;

		private MvxObservableCollection<LogOverview> logs;

		private Task refreshTask;

		private Settings settings;

		private DateTime? srchDate;

		private string srchName;

		private Result srchSelResult;

		#endregion

		#region Constructors and Destructors

		public RootViewModel(IExplorer explorer, ITester tester)
		{
			this.explorer = explorer;
			this.manager = Mvx.Resolve<Manager>();
			this.tester = tester;
			this.AllResults = ResultHelper.GetAllResults();
			this.srchSelResult = this.AllResults.First();
			this.logs = manager.LogOverview;
			this.logger = Logger.Instance;
			this.IsRerunAvailable = true;
		}

		#endregion

		#region Public Properties

		public List<Result> AllResults { get; }

		public IMvxCommand CmdClearFilter
		{
			get
			{
				return new MvxCommand(this.ClearFilter);
			}
		}

		public IMvxCommand<Log> CmdDeleteOne
		{
			get
			{
				return new MvxCommand<Log>(this.DeleteOne);
			}
		}

		public IMvxCommand CmdDeleteSelected
		{
			get
			{
				return new MvxCommand(this.DeleteSelected);
			}
		}

		public IMvxCommand CmdExport
		{
			get
			{
				return new MvxCommand(() => this.manager.Export(this.settings.ExportPath));
			}
		}

		public IMvxCommand CmdExportDir
		{
			get
			{
				return new MvxCommand(() => this.manager.Export(this.settings.ExportPath, false));
			}
		}

		public IMvxCommand CmdNavigateSettings
		{
			get
			{
				return new MvxCommand(() => this.ShowViewModel<SettingsViewModel>());
			}
		}

		public IMvxCommand CmdRefresh
		{
			get
			{
				return new MvxCommand(this.Refresh);
			}
		}

		public IMvxCommand<Log> CmdRerun
		{
			get
			{
				return new MvxCommand<Log>(this.RerunOne);
			}
		}

		public IMvxCommand CmdRerunSelected
		{
			get
			{
				return this.isRerunAvailable ? new MvxCommand(this.RerunSelected) : new MvxCommand(this.CancelRerun);
			}
		}

		public IMvxCommand<string> CmdStartProcess
		{
			get
			{
				return new MvxCommand<string>(path => { FileHelper.StartProcess(path); });
			}
		}

		public string FilterCounter => $"({this.logs?.Count ?? 0}/{this.manager.LogOverview?.Count ?? 0})";

		public bool IsRerunAvailable
		{
			get
			{
				return this.isRerunAvailable;
			}
			private set
			{
				this.isRerunAvailable = value;
				this.RaisePropertyChanged(() => this.IsRerunAvailable);
				this.RaisePropertyChanged(() => this.RerunSelectedButtonName);
				this.RaisePropertyChanged(() => this.CmdRerunSelected);
			}
		}

		public MvxObservableCollection<LogOverview> Logs
		{
			get
			{
				return this.logs;
			}

			set
			{
				this.logs = value;
				this.RaisePropertyChanged(() => this.Logs);
			}
		}

		public string RerunSelectedButtonName
		{
			get
			{
				if (this.isRerunAvailable)
				{
					return ElementsName.RerunSelectedAvailable;
				}
				return ElementsName.RerunSelectedUnavailable;
			}
		}

		public DateTime? SrchDate
		{
			get
			{
				return this.srchDate;
			}
			set
			{
				this.srchDate = value;
				this.Filter();
				this.RaisePropertyChanged(() => this.SrchDate);
			}
		}

		public string SrchName
		{
			get
			{
				return this.srchName;
			}
			set
			{
				this.srchName = value;
				this.Filter();
				this.RaisePropertyChanged(() => this.SrchName);
			}
		}

		public Result SrchSelResult
		{
			get
			{
				return this.srchSelResult;
			}
			set
			{
				this.srchSelResult = value;
				this.Filter();
				this.RaisePropertyChanged(() => this.SrchSelResult);
			}
		}

		#endregion

		#region Public Methods and Operators

		public override void ViewAppearing()
		{
			this.settings = Mvx.Resolve<Repository>().Settings;
			this.Refresh();

			base.ViewAppearing();
		}

		#endregion

		#region Methods

		private async void CancelRerun()
		{
			if (await Popup.ShowConfirmAsync(Messages.CancelRerunQuesion))
			{
				this.cts?.Cancel();
			}
		}

		private void ClearFilter()
		{
			this.srchName = string.Empty;
			this.srchSelResult = this.AllResults[0];
			this.srchDate = null;
			this.RaisePropertyChanged(() => this.SrchName);
			this.RaisePropertyChanged(() => this.SrchSelResult);
			this.RaisePropertyChanged(() => this.SrchDate);
			this.Filter();
		}

		private async void DeleteOne(Log log)
		{
			var delete = await Popup.ShowConfirmAsync(Messages.GetDeleteOneLogQuestion(log.Name, log.StartTimeString));
			if (delete)
			{
				this.manager.DeleteLog(log);
				this.RaisePropertyChanged(() => this.FilterCounter);
				return;
			}
			this.logger.AddMessage(Messages.DeleteAborted);
		}

		private async void DeleteSelected()
		{
			var delete = await Popup.ShowConfirmAsync(Messages.DeleteSelectedLogsQuestion);
			if (delete)
			{
				this.manager.DeleteSelectedLogs();
				this.RaisePropertyChanged(() => this.FilterCounter);
				return;
			}
			this.logger.AddMessage(Messages.DeleteAborted);
		}

		private void Filter()
		{
			if (this.refreshTask?.IsCompleted == false)
			{
				return;
			}

			var isActie = false;
			IEnumerable<LogOverview> searchLogs = this.manager.LogOverview.ToArray();

			if (!string.IsNullOrEmpty(this.srchName))
			{
				isActie = true;
				searchLogs = searchLogs.Where(log => log.Log.Name.ContainsString(this.srchName));
			}

			if (!string.IsNullOrEmpty(this.srchSelResult?.Value))
			{
				isActie = true;
				searchLogs = searchLogs.Where(log => log.Log.Result.ContainsString(this.srchSelResult.Value));
			}

			if (this.srchDate.HasValue)
			{
				isActie = true;
				searchLogs =
					searchLogs.Where(log => log.History.Any(history => DateTime.Compare(this.srchDate.Value, history.StartTime) <= 0));
			}

			if (isActie)
			{
				if (this.Logs?.Count == searchLogs.Count()
				    && !this.Logs.Except(searchLogs).Any())
				{
					return;
				}
				var backup = new MvxObservableCollection<LogOverview>(this.manager.LogOverview);
				this.Logs.SwitchTo(searchLogs);

				this.manager.LogOverview = backup;
				this.RaisePropertyChanged(() => this.FilterCounter);
				return;
			}
			if (this.Logs?.Count == this.manager.LogOverview?.Count())
			{
				return;
			}

			this.Logs.SwitchTo(this.manager.LogOverview);
			this.RaisePropertyChanged(() => this.FilterCounter);
		}

		private void ReasignCollections(MvxObservableCollection<LogOverview> newCollection)
		{
			this.manager.LogOverview = newCollection;
			this.Logs = this.manager.LogOverview;
		}

		private async void Refresh()
		{
			this.ReasignCollections(new MvxObservableCollection<LogOverview>());
			refreshTask = this.explorer.PopulateLogsRootAsync(
				this.manager.LogOverview,
				this.settings.RootLogsPath,
				() => this.RaisePropertyChanged(() => this.FilterCounter));
			await refreshTask;

			this.Filter();
			this.RaisePropertyChanged(() => this.FilterCounter);
		}

		private async void RerunOne(Log log)
		{
			this.IsRerunAvailable = false;
			this.cts = new CancellationTokenSource();
			try
			{
				if (await this.tester.RerunAsync(log, this.settings, this.cts.Token))
				{
					var updatedHistory = this.explorer.GetLogHistory(FileHelper.GetParent(log.DirPath));
					this.manager.UpdateOverview(updatedHistory);
				}
			}
			catch (OperationCanceledException)
			{
				this.logger.AddMessage(Messages.RerunCanceled);
			}
			this.cts = null;
			this.IsRerunAvailable = true;
		}

		private async void RerunSelected()
		{
			var selectedLogs = this.manager.GetSelectedLogs();
			if (selectedLogs == null
			    || !selectedLogs.Any())
			{
				this.logger.AddMessage(Messages.NothingSelected);
				return;
			}
			this.IsRerunAvailable = false;
			this.cts = new CancellationTokenSource();
			try
			{
				await this.tester.RerunQueueAwait(selectedLogs, this.settings, this.cts.Token);
			}
			catch (OperationCanceledException)
			{
				this.logger.AddMessage(Messages.RerunCanceled);
			}
			this.cts = null;
			this.IsRerunAvailable = true;
		}

		#endregion
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Core;
using LogExplorer.Services.Extensions;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

#endregion

namespace LogExplorer.ViewModels
{
	public class RootViewModel : MvxViewModel
	{
		#region Fields

		private readonly IExplorer explorer;

        private readonly IManager manager;

        private readonly ITester tester;

        private MvxObservableCollection<LogOverview> logs;

		private Settings settings;

		private DateTime? srchDate;

		private string srchName;

		private Result srchSelResult;

		#endregion

		#region Constructors and Destructors

		public RootViewModel(IExplorer explorer, IManager manager, ITester tester)
		{
			this.explorer = explorer;
			this.manager = manager;
		    this.tester = tester;
			this.AllResults = ResultHelper.GetAllResults();
			this.srchSelResult = this.AllResults.First();
			this.logs = manager.LogOverview;
		}

		#endregion

		#region Public Properties

		public List<Result> AllResults { get; }

	    public override void ViewAppearing()
	    {
	        this.settings = Mvx.Resolve<Repository>().GetSettings();
            this.Refresh();

            base.ViewDisappeared();
        }

        public IMvxCommand CmdClearFilter
		{
			get
			{
				return new MvxCommand(this.ClearFilter);
			}
		}

		public IMvxCommand CmdExport
		{
			get
			{
				return new MvxCommand(this.Export);
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

		public IMvxCommand<string> CmdStartProcess
		{
			get
			{
				return new MvxCommand<string>(FileHelper.StartProcess);
			}
		}
        public IMvxCommand<Log> CmdRerun
        {
            get
            {
                return new MvxCommand<Log>(log=>this.tester.Rerun(log, this.settings));
            }
        }

        public string FilterCounter => $"({this.logs?.Count ?? 0}/{this.manager.LogOverview?.Count ?? 0})";

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
        
		#region Methods

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

		private void Export()
		{
			this.manager.Export(this.settings.ExportPath);
		}

		private void Filter()
		{
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

				searchLogs =
					searchLogs.Where(log => log.History.Any(history => history.Result.ContainsString(this.srchSelResult.Value)));
			}

			if (this.srchDate.HasValue)
			{
				isActie = true;

				searchLogs =
					searchLogs.Where(log => log.History.Any(history => DateTime.Compare(this.srchDate.Value, history.StartTime) <= 0));
			}

			if (isActie)
			{
				if (this.Logs?.Count == searchLogs.Count())
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
			await
				this.explorer.PopulateLogsRootAsync(
					this.manager.LogOverview,
					this.settings.RootLogsPath,
					() => this.RaisePropertyChanged(() => this.FilterCounter));

			this.Filter();
			this.RaisePropertyChanged(() => this.FilterCounter);
		}

		#endregion
	}
}
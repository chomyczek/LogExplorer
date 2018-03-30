// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Core;
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

		private Settings settings;

		#endregion

		#region Constructors and Destructors

		public RootViewModel(IExplorer explorer, IManager manager)
		{
			this.explorer = explorer;
			this.manager = manager;
		}

		#endregion

		#region Public Properties

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

		public IMvxCommand CmdExport
		{
			get
			{
				return new MvxCommand(this.Export);
			}
		}

		private void Export()
		{
			var selectedLogs = this.manager.GetSelectedLogs();
			this.manager.Export(selectedLogs, this.settings.ExportPath);
		}



		public IMvxCommand<string> CmdStartProcess
		{
			get
			{
				return new MvxCommand<string>(FileHelper.StartProcess);
			}
		}

		public MvxObservableCollection<LogOverview> Logs {
			get
			{
				return this.manager.LogOverview;
			}

			set
			{
				this.manager.LogOverview = value;
                RaisePropertyChanged(() => Logs);
            }
		}

		#endregion

		#region Public Methods and Operators

		public override void Start()
		{
			this.Refresh();
			base.Start();
		}

		#endregion

		#region Methods

		private void Refresh()
		{
			this.settings = Mvx.Resolve<Repository>().GetSettings();
			this.Logs = this.explorer.GetLogsRoot(this.settings.RootLogsPath);
			this.RaisePropertyChanged(() => this.Logs);
		}

		#endregion
	}
}
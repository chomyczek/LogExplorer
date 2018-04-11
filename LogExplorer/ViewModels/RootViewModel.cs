// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

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
	    private string nameSrch;

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

        public string NameSrch
        {
            get
            {
                return nameSrch;
            }
            set
            {
                this.nameSrch = value;
                this.Filtr();
                this.RaisePropertyChanged(() => this.NameSrch);
            }
        }

	    private string resultSrch;

        public string ResultSrch
        {
            get
            {
                return resultSrch;
            }
            set
            {
                this.resultSrch = value;
                this.Filtr();
                this.RaisePropertyChanged(() => this.ResultSrch);
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

	    private MvxObservableCollection<LogOverview> logs;

        public MvxObservableCollection<LogOverview> Logs {
			get
			{
				return this.logs;
			}

			set
			{
				this.logs = value;
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
            this.manager.LogOverview = this.explorer.GetLogsRoot(this.settings.RootLogsPath);
		    this.Filtr();
		}

	    private void Filtr()
	    {
	        var isActie = false;
	        IEnumerable<LogOverview> searchLogs = this.manager.LogOverview;
            if (!string.IsNullOrEmpty(this.nameSrch))
	        {
	            isActie = true;
                searchLogs= searchLogs.Where(log => log.Log.Name.ContainsString(this.nameSrch));
            }

	        if (!string.IsNullOrEmpty(this.resultSrch))
	        {
                isActie = true;

                searchLogs = searchLogs.Where(log => log.History.Any(history=>history.Result.ContainsString(this.resultSrch)));
            }

	        if (isActie)
	        {
                this.Logs = new MvxObservableCollection<LogOverview>(searchLogs);
                return;
            }

            this.Logs = this.manager.LogOverview;
            
	    }

		#endregion
	}
}
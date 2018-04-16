// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
		    this.AllResults = ResultHelper.GetAllResults();
			this.selResultSrch = this.AllResults.First();
			this.logs = manager.LogOverview;


        }

        #endregion

        #region Public Properties

	    //private string filterCounter;

        public string FilterCounter
        //{
        //    get
        //    {
        //        return nameSrch;
        //    }
        //    set
        //    {
        //        this.nameSrch = value;
        //        this.Filter();
        //        this.RaisePropertyChanged(() => this.NameSrch);
        //    }
        //}
        => $"({this.logs?.Count ?? 0}/{this.manager.LogOverview?.Count ?? 0})";

        public string NameSrch
        {
            get
            {
                return this.nameSrch;
            }
            set
            {
                this.nameSrch = value;
                this.Filter();
                this.RaisePropertyChanged(() => this.NameSrch);
            }
        }

	    private DateTime? dateSrch;


        public DateTime? DateSrch
        {
            get
            {
                return this.dateSrch;
            }
            set
            {
                this.dateSrch = value;
                this.Filter();
                this.RaisePropertyChanged(() => this.DateSrch);
            }
        }

        private Result selResultSrch;

        public Result SelResultSrch
        {
            get
            {
                return this.selResultSrch;
            }
            set
            {
                this.selResultSrch = value;
                this.Filter();
                this.RaisePropertyChanged(() => this.SelResultSrch);
            }
        }


	    public List<Result> AllResults { get; }

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
        public IMvxCommand CmdClearFilter
        {
            get
            {
                return new MvxCommand(this.ClearFilter);
            }
        }

	    private void ClearFilter()
	    {
            this.nameSrch = string.Empty;
            this.selResultSrch = this.AllResults[0];
	        this.dateSrch = null;
            this.RaisePropertyChanged(() => this.NameSrch);
            this.RaisePropertyChanged(() => this.SelResultSrch);
            this.RaisePropertyChanged(() => this.DateSrch);
            this.Filter();
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
				this.RaisePropertyChanged(() => this.Logs);
            }
		}

        #endregion

        #region Public Methods and Operators

        public override void Start()
        {
            base.Start();
            this.Refresh();
        }

        #endregion

        #region Methods

        private async void Refresh()
		{
			this.settings = Mvx.Resolve<Repository>().GetSettings();
	        this.ReasignCollections(new MvxObservableCollection<LogOverview>());
			await
		        this.explorer.PopulateLogsRootAsync(
			        this.manager.LogOverview,
			        this.settings.RootLogsPath,
			        () => this.RaisePropertyChanged(() => this.FilterCounter));
			
			this.Filter();
            this.RaisePropertyChanged(() => this.FilterCounter);
        }

		private void ReasignCollections(MvxObservableCollection<LogOverview> newCollection)
		{
			this.manager.LogOverview = newCollection;
			this.Logs = this.manager.LogOverview;
		}

        private void Filter()
	    {
	        var isActie = false;
	        IEnumerable<LogOverview> searchLogs = this.manager.LogOverview;

            if (!string.IsNullOrEmpty(this.nameSrch))
	        {
	            isActie = true;
                searchLogs= searchLogs.Where(log => log.Log.Name.ContainsString(this.nameSrch));
            }

	        if (!string.IsNullOrEmpty(this.selResultSrch?.Value))
	        {
                isActie = true;

                searchLogs = searchLogs.Where(log => log.History.Any(history=>history.Result.ContainsString(this.selResultSrch.Value)));
            }

            if (this.dateSrch.HasValue)
            {
                isActie = true;

                searchLogs =
                    searchLogs.Where(
                        log => log.History.Any(history => DateTime.Compare(this.dateSrch.Value, history.StartTime) <= 0));
            }

            if (isActie)
	        {
	            if (this.Logs?.Count == searchLogs.Count())
	            {
	                return;
	            }
                this.Logs = new MvxObservableCollection<LogOverview>(searchLogs);
                this.RaisePropertyChanged(() => this.FilterCounter);
                return;
            }
            if (this.Logs?.Count == this.manager.LogOverview?.Count())
            {
                return;
            }

            this.Logs = this.manager.LogOverview;
            this.RaisePropertyChanged(() => this.FilterCounter);

        }


		#endregion
	}
}
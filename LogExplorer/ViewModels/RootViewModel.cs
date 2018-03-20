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

		private Settings settings;

		#endregion

		#region Constructors and Destructors

		public RootViewModel(IExplorer explorer)
		{
			//TODO: Set start directory
			this.explorer = explorer;
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

		public IMvxCommand<string> CmdStartProcess
		{
			get
			{
				return new MvxCommand<string>(FileHelper.StartProcess);
			}
		}

		public List<Log> Logs { get; set; }

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
			this.Logs = this.explorer.GetLogsRoot(this.settings.RootLogsPath).ToList();
			this.RaisePropertyChanged(() => this.Logs);
		}

		#endregion
	}
}
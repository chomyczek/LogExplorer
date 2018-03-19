// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;

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
			this.Logs = new List<Log>();
		}

		#endregion

		#region Public Properties

		public IMvxCommand<string> CmdStartProcess
		{
			get
			{
				return new MvxCommand<string>(FileHelper.StartProcess);
			}
		}

		public List<Log> Logs { get; set; }

		public IMvxCommand CmdNavigateSettings
		{
			get
			{
				return new MvxCommand(() => this.ShowViewModel<SettingsViewModel>());
			}
		}

		#endregion

		#region Public Methods and Operators

		public override void Start()
		{
			this.settings = Mvx.Resolve<Repository>().GetSettings();
			this.Refresh();
			base.Start();
		}

		#endregion

		#region Methods

		private void Refresh()
		{
			this.Logs = this.explorer.GetLogsRoot(this.settings.LogsPath);
		}

		#endregion
	}
}
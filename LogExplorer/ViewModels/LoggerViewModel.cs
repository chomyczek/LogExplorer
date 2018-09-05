// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using LogExplorer.Models;
using LogExplorer.Services.Core;
using LogExplorer.Services.OutputSystem;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

#endregion

namespace LogExplorer.ViewModels
{
	public class LoggerViewModel : MvxViewModel
	{
		#region Fields

		private readonly Logger logger;

		private Settings settings;

		#endregion

		#region Constructors and Destructors

		public LoggerViewModel()
		{
			Logger.PrepareInstance(() => { this.RaisePropertyChanged(() => this.LoggerBox); });
			this.logger = Logger.Instance;
			this.settings = Mvx.Resolve<Repository>().Settings;
		}

		#endregion

		#region Public Properties

		public string LoggerBox => this.logger.Message;

		public Settings Settings
		{
			get
			{
				return this.settings;
			}
		}

		#endregion
	}
}
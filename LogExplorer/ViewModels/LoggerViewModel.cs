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

		private const double EnabledHeight = 100;
		private const double DisabledHeight = 0;
		private readonly Logger logger;

		private double height = EnabledHeight;

		private Settings settings;

		#endregion

		#region Constructors and Destructors

		public LoggerViewModel()
		{
			Logger.PrepareInstance(() => { this.RaisePropertyChanged(() => this.LoggerBox); });
			this.logger = Logger.Instance;
			this.settings = Mvx.Resolve<Repository>().Settings;
			if (this.settings.IsLoggerEnabled)
			{
				this.Height = EnabledHeight;
			}
			else
			{
				this.Height = DisabledHeight;
			}
		}

		#endregion

		#region Public Properties

		public double Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				this.RaisePropertyChanged(() => this.Height);
			}
		}

		public string LoggerBox => this.logger.Message;

		#endregion
	}
}
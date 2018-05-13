// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using LogExplorer.Services.OutputSystem;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.ViewModels
{
	public class LoggerViewModel : MvxViewModel
	{
		#region Fields

		private readonly Logger logger;

		private double height = 100;

		#endregion

		#region Constructors and Destructors

		public LoggerViewModel()
		{
			Logger.PrepareInstance(() => { this.RaisePropertyChanged(() => this.LoggerBox); });
			this.logger = Logger.Instance;
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
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

		#endregion

		#region Constructors and Destructors

		public LoggerViewModel()
		{
			Logger.PrepareInstance(() => { this.RaisePropertyChanged(() => this.LoggerBox); });
			this.logger = Logger.Instance;
		}

		#endregion

		#region Public Properties

		public string LoggerBox
		{
			get
			{
				return this.logger.Message;
			}
			//Empty set to avoid writing to logger
			set
			{
			}
		}

		#endregion
	}
}
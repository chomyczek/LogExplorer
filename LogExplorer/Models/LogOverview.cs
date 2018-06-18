// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Models
{
	public class LogOverview : MvxNotifyPropertyChanged
	{
		#region Fields

		private MvxObservableCollection<Log> history;

		#endregion

		#region Public Properties

		public MvxObservableCollection<Log> History
		{
			get
			{
				return this.history;
			}

			set
			{
				this.history = value;
				this.RaisePropertyChanged(() => this.History);
				this.RaisePropertyChanged(() => this.Log);
			}
		}

		public Log Log => this.History[0];

		#endregion
	}
}
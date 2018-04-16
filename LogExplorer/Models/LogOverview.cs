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
				RaisePropertyChanged(() => History);
			}
		}

		public Log Log => History[0];

		#endregion
	}
}
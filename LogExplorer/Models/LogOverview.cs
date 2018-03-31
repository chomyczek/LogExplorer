using System;
using System.Collections.Generic;
using System.Windows.Media;
using MvvmCross.Core.ViewModels;

namespace LogExplorer.Models
{
    public class LogOverview: MvxNotifyPropertyChanged
    {
        #region Public Properties

        private MvxObservableCollection<Log> history;
        public MvxObservableCollection<Log> History
        {
            get { return this.history; }

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
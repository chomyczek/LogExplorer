using System;
using System.Collections.Generic;
using System.Windows.Media;
using MvvmCross.Core.ViewModels;

namespace LogExplorer.Models
{
    public class LogOverview: MvxNotifyPropertyChanged
    {
        #region Public Properties

        public string DirPath => Log.DirPath;

        public string DurationString => Log.DurationString;
        
        public MvxObservableCollection<Log> History { get; set; }

        public DateTime StartTime => Log.StartTime;

        public bool IsSelected
        {
            get
            {
                return Log.IsSelected;
            }
            set { Log.IsSelected = value; RaisePropertyChanged(() => IsSelected); }
        }


        public string LogPath => Log.LogPath;

        public string Name => Log.Name;

        public string Result => Log.Result;

        public Brush ResultColor => Log.ResultColor;

        public string StartTimeString => Log.StartTimeString;

        //when log.isSelected is updated, get IsSelected remain the same
        private Log Log => History[0];

        #endregion
    }
}
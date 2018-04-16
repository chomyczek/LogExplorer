// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Media;
using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Models
{
	public class Log: MvxNotifyPropertyChanged
	{
	    public const string DateFormat = "g";
		#region Public Properties

		public string DirPath { get; set; }

		public string DirTime { get; set; }

		public TimeSpan Duration { get; set; }

		public string DurationString => this.Duration.ToString(@"hh\:mm\:ss");

	    private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set {
                this.isSelected = value;
                RaisePropertyChanged(() => this.IsSelected);
            }
        }

        public string LogPath { get; set; }

		public string Name { get; set; }

	    public string Result => this.ResultContainer.Name;

	    public Brush ResultColor => this.ResultContainer.Brush;

        public DateTime StartTime { get; set; }

		public string StartTimeString => this.StartTime.ToString(DateFormat);

	    public Result ResultContainer { get; set; }

        #endregion
    }
}
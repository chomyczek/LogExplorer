// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Windows.Media;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Models
{
	public class Log : MvxNotifyPropertyChanged
	{
		#region Constants

		public const string DateFormat = "g";

		#endregion

		#region Fields

		private bool isSelected;

		#endregion

		#region Public Properties

		public string DirPath { get; set; }

		public string DirTime { get; set; }

		public TimeSpan Duration { get; set; }

		public string DurationString => this.Duration.ToString(@"hh\:mm\:ss");

		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				this.isSelected = value;
				RaisePropertyChanged(() => this.IsSelected);
			}
		}

		public string LogPath { get; set; }

		public string Name { get; set; }

		public string Result => this.ResultContainer.Name;

		public Brush ResultColor => this.ResultContainer.Brush;

		public Result ResultContainer { get; set; }

		public DateTime StartTime { get; set; }

		public string StartTimeString => this.StartTime.ToString(DateFormat);

		#endregion
	}
}
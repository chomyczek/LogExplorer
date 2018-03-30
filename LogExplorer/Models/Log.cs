// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Media;

#endregion

namespace LogExplorer.Models
{
	public class Log
	{
		#region Public Properties

		public string DirPath { get; set; }

		public string DirTime { get; set; }

		public TimeSpan Duration { get; set; }

		public string DurationString => this.Duration.ToString(@"hh\:mm\:ss");
        
		public bool IsSelected { get; set; }

		public string LogPath { get; set; }

		public string Name { get; set; }

		public string Result { get; set; }

		public Brush ResultColor { get; set; }

		public DateTime StartTime { get; set; }

		public string StartTimeString => this.StartTime.ToString("g");

		#endregion
	}
}
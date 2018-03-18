// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;

#endregion

namespace LogExplorer.Models
{
	public class Log
	{
		#region Public Properties

		public string Name { get; set; }

		public string Result { get; set; }

		public DateTime StartTime { get; set; }

		public string DirPath { get; set; }

		public string DirTime { get; set; }

		public string LogPath => $@"{this.DirPath}\LOG_{this.DirTime}.html";

		#endregion
	}
}
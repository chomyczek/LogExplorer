﻿// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

namespace LogExplorer.Models
{
	public class Settings
	{
		#region Constructors and Destructors

		public Settings()
		{
			this.Height = string.Empty;
			this.RootLogsPath = @"C:\History\";
			this.PositionX = string.Empty;
			this.PositionY = string.Empty;
			this.Width = string.Empty;
			this.ExportPath = @"C:\ExportedLogs\";
		}

		#endregion

		#region Public Properties

		public string ExportPath { get; set; }

		public string Height { get; set; }

		public string PositionX { get; set; }

		public string PositionY { get; set; }

		public string RootLogsPath { get; set; }

		public string Width { get; set; }

		#endregion
	}
}
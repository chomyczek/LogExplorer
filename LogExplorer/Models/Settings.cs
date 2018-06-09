// LogExplorer
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
			this.RootLogsPath = string.Empty;
			this.PositionX = string.Empty;
			this.PositionY = string.Empty;
			this.Width = string.Empty;
			this.ExportPath = string.Empty;
			this.TesterPath = string.Empty;
			this.ConfigMode = 0;
			this.CustomConfigPath = string.Empty;
			this.IsHiddenTester = false;
			this.LoggerMemory = 100;
			this.IsLoggerShowDetails = false;
		}

		#endregion

		#region Public Properties

		public int ConfigMode { get; set; }

		public string CustomConfigPath { get; set; }

		public string ExportPath { get; set; }

		public string Height { get; set; }

		public bool IsHiddenTester { get; set; }

		public int LoggerMemory { get; set; }

		public bool IsLoggerShowDetails { get; set; }

		public string PositionX { get; set; }

		public string PositionY { get; set; }

		public string RootLogsPath { get; set; }

		public string TesterPath { get; set; }

		public string Width { get; set; }

		#endregion
	}
}
﻿// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Interfaces;

#endregion

namespace LogExplorer.Services.Core
{
	public class Explorer : IExplorer
	{
		#region Public Methods and Operators

		/// <summary>
		/// todo: This is basic version, try catch
		/// </summary>
		public List<Log> GetLogsRoot(string path)
		{
			var logDirs = Directory.GetDirectories(path);
			var logs = new List<Log>();
			foreach (var dir in logDirs)
			{
				var logDir = Directory.GetDirectories(dir).FirstOrDefault();

				if (string.IsNullOrEmpty(logDir))
				{
					continue;
				}

				var log = new Log
				          {
					          Name = Path.GetFileName(dir),
					          Result = this.GetResult(logDir),
					          StartTime = Directory.GetCreationTime(logDir),
					          DirPath = logDir,
							  DirTime = Path.GetFileName(logDir)
				};
				logs.Add(log);
			}
			return logs;
		}

		private string GetResult(string path)
		{
			var files = Directory.GetFiles(path);
			var fileNames = files.Select(Path.GetFileName);

			if (fileNames.Contains("Failed"))
			{
				return "Failed";
			}
			if (fileNames.Contains("Passed"))
			{
				return "Passed";
			}
			if (fileNames.Contains("Exception"))
			{
				return "Exception";
			}
			if (fileNames.Contains("Workaround"))
			{
				return "Workaround";
			}
			if (fileNames.Contains("Warning"))
			{
				return "Warning";
			}
			return "Unknown";
		}

		#endregion
	}
}
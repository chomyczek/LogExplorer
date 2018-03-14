// LogExplorer
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
		/// todo: This is basic version
		/// </summary>
		public List<Log> GetAllLogs()
		{
			const string LogsPath = @"C:\CONFIG TMP\History2";
			var logDirs = Directory.GetDirectories(LogsPath);
			var logs = new List<Log>();
			foreach (var dir in logDirs)
			{
				var logDir = Directory.GetDirectories(dir).FirstOrDefault();
				if (string.IsNullOrEmpty(logDir))
				{
					continue;
				}
				var files = Directory.GetFiles(logDir);
				string res;
				if (files.Contains("Failed"))
				{
					res = "Filed";
				}
				else if (files.Contains("Passed"))
				{
					res = "Passed";
				}
				else
				{
					res = "Unknown";
				}

				var log = new Log
				          {
					          Name = Path.GetDirectoryName(dir),
					          Result = res,
					          StartTime = Directory.GetCreationTime(logDir),
					          DirPath = logDir
				          };
				logs.Add(log);
			}
			return logs;
		}

		#endregion
	}
}
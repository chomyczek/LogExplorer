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
				var fileNames = files.Select(Path.GetFileName);
;				string res;
				if (fileNames.Contains("Failed"))
				{
					res = "Failed";
				}
				else if (fileNames.Contains("Passed"))
				{
					res = "Passed";
				}
				else if (fileNames.Contains("Exception"))
				{
					res = "Exception";
				}
				else
				{
					res = "Unknown";
				}

				var log = new Log
				          {
					          Name = Path.GetFileName(dir),
					          Result = res,
					          StartTime = Directory.GetCreationTime(logDir),
					          DirPath = logDir,
							  DirTime = Path.GetFileName(logDir)
				};
				logs.Add(log);
			}
			return logs;
		}

		#endregion
	}
}
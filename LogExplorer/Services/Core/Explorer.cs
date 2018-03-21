// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;
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
			var logs = new List<Log>();

			if (!Directory.Exists(path))
			{
				//todo: warning
				return logs;
			}

			var logDirs = Directory.GetDirectories(path);

			foreach (var dir in logDirs)
			{
				var logDir = Directory.GetDirectories(dir).OrderByDescending(Directory.GetCreationTime).FirstOrDefault();

				if (string.IsNullOrEmpty(logDir))
				{
					continue;
				}

				var result = this.GetResult(logDir);

				var log = new Log
				          {
					          Name = Path.GetFileName(dir),
					          Result = result,
					          StartTime = Directory.GetCreationTime(logDir),
					          DirPath = logDir,
					          DirTime = Path.GetFileName(logDir),
					          LogPath = this.GetLogPath(logDir),
					          ResultColor = ResultHelper.GetColor(result)
				          };
				logs.Add(log);
			}
			return logs.OrderByDescending(l => l.StartTime).ToList();
		}

		#endregion

		#region Methods

		private string GetLogPath(string path)
		{
			var files = Directory.GetFiles(path).Select(Path.GetFileName);
			var logName = files.FirstOrDefault(f => f.EndsWith($@"{Path.GetFileName(path)}.html") && f.StartsWith("LOG_"));
			var logPath = string.IsNullOrEmpty(logName) ? string.Empty : $@"{path}\{logName}";
			return logPath;
		}

		private string GetResult(string path)
		{
			var files = Directory.GetFiles(path);
			var fileNames = files.Select(Path.GetFileName);
			var result = ResultHelper.GetResult(fileNames);
			return result;
		}

		#endregion
	}
}
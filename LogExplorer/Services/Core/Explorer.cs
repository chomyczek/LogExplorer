// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
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
			//todo remove debug
			var start = DateTime.Now;
			
			
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
				var logPath = this.GetLogPath(logDir);
				var startTime = Directory.GetCreationTime(logDir);

				var log = new Log
				          {
					          Name = Path.GetFileName(dir),
					          Result = result,
					          StartTime = startTime,
					          DirPath = logDir,
					          DirTime = Path.GetFileName(logDir),
					          LogPath = logPath,
					          ResultColor = ResultHelper.GetColor(result),
							  Duration = this.GetDuration(startTime, logPath),
							  History = this.GetLogHistory(dir)//new List<Log>()
				          };
				logs.Add(log);
			}
			//todo remove debug
			var diff = DateTime.Now.Subtract(start);
			Console.WriteLine("GetLogsRoot took: {0}s", diff.TotalSeconds);
			return logs.OrderByDescending(l => l.StartTime).ToList();
		}

		private TimeSpan GetDuration(DateTime startTime, string logPath)
		{
			if (string.IsNullOrEmpty(logPath)
			    || !File.Exists(logPath))
			{
				return new TimeSpan();
			}
			var logTime = File.GetCreationTime(logPath);
			return logTime - startTime;
		}

		public List<Log> GetLogHistory(string path)
		{
			var logs = new List<Log>();
			var logDirs = Directory.GetDirectories(path);
			var name = Path.GetFileName(path);

			foreach (var dir in logDirs)
			{

				var result = this.GetResult(dir);
				var logPath = this.GetLogPath(dir);
				var startTime = Directory.GetCreationTime(dir);

				var log = new Log
				{
					Name = name,
					Result = result,
					StartTime = startTime,
					DirPath = dir,
					DirTime = Path.GetFileName(dir),
					LogPath = logPath,
					Duration = this.GetDuration(startTime, logPath),
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
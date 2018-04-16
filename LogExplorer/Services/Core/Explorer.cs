// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Core
{
	public class Explorer : IExplorer
	{
		#region Public Methods and Operators

		public Task PopulateLogsRootAsync(
			MvxObservableCollection<LogOverview> logOverviews,
			string path,
			Action propertyChanges)
		{
			//todo try/catch
			return Task.Run(
				() =>
				{
					//todo remove debug
					var start = DateTime.Now;

					if (!Directory.Exists(path))
					{
						//todo: warning
						//return new MvxObservableCollection<LogOverview>();
						return;
					}

					var logDirs = Directory.GetDirectories(path).OrderByDescending(Directory.GetLastWriteTime);

					foreach (var dir in logDirs)
					{
						var logDir = Directory.GetDirectories(dir).OrderByDescending(Directory.GetCreationTime).FirstOrDefault();

						if (string.IsNullOrEmpty(logDir))
						{
							continue;
						}

						var logOverview = new LogOverview { History = this.GetLogHistory(dir) };
						logOverviews.Add(logOverview);
						propertyChanges.Invoke();
					}

					//todo remove debug
					var diff = DateTime.Now.Subtract(start);
					Console.WriteLine("GetLogsRoot took: {0}s", diff.TotalSeconds);
				});
		}

		#endregion

		#region Methods

		private Log CollectLogInfo(string path, string name)
		{
			var logPath = this.GetLogPath(path);
			var startTime = Directory.GetCreationTime(path);

			var log = new Log
			          {
				          Name = name,
				          ResultContainer = this.GetResult(path),
				          StartTime = startTime,
				          DirPath = path,
				          DirTime = Path.GetFileName(path),
				          LogPath = logPath,
				          Duration = this.GetDuration(startTime, logPath)
			          };
			return log;
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

		private MvxObservableCollection<Log> GetLogHistory(string path)
		{
			var logs = new MvxObservableCollection<Log>();
			var logDirs = Directory.GetDirectories(path);
			var name = Path.GetFileName(path);

			foreach (var dir in logDirs)
			{
				var log = this.CollectLogInfo(dir, name);

				logs.Add(log);
			}

			return new MvxObservableCollection<Log>(logs.OrderByDescending(l => l.StartTime));
		}

		private string GetLogPath(string path)
		{
			var files = Directory.GetFiles(path).Select(Path.GetFileName);
			var logName = files.FirstOrDefault(f => f.EndsWith($@"{Path.GetFileName(path)}.html") && f.StartsWith("LOG_"));
			var logPath = string.IsNullOrEmpty(logName) ? string.Empty : $@"{path}\{logName}";
			return logPath;
		}

		private Result GetResult(string path)
		{
			var files = Directory.GetFiles(path);
			var fileNames = files.Select(Path.GetFileName);
			var result = ResultHelper.GetResult(fileNames);
			return result;
		}

		#endregion
	}
}
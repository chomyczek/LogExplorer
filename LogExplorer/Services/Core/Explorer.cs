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
using LogExplorer.Services.OutputSystem;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Core
{
	public class Explorer : IExplorer
	{
		#region Fields

		private readonly Logger logger;

		#endregion

		#region Constructors and Destructors

		public Explorer()
		{
			this.logger = Logger.Instance;
		}

		#endregion

		#region Public Methods and Operators

		public Task PopulateLogsRootAsync(
			MvxObservableCollection<LogOverview> logOverviews,
			string path,
			Action propertyChanges)
		{
			return Task.Run(
				() =>
				{
					var start = DateTime.Now;

					if (!Directory.Exists(path))
					{
						Popup.ShowWarning(Messages.RootDirDoesntExist);
						return;
					}

					var logDirs = Directory.GetDirectories(path).OrderByDescending(Directory.GetLastWriteTime);

					foreach (var dir in logDirs)
					{
						this.logger.AddDetailMessage(Messages.GetScanningDir(dir));
						var logDir = Directory.GetDirectories(dir).OrderByDescending(Directory.GetCreationTime).FirstOrDefault();

						if (string.IsNullOrEmpty(logDir))
						{
							this.logger.AddDetailMessage(Messages.GetDirIsEmpty(logDir));
							continue;
						}

						var logOverview = new LogOverview { History = this.GetLogHistory(dir) };
						logOverviews.Add(logOverview);
						propertyChanges.Invoke();
					}
					
					var diff = DateTime.Now.Subtract(start);
					this.logger.AddMessage(Messages.GetCollectingLogsDuration(diff.TotalSeconds));
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

		public MvxObservableCollection<Log> GetLogHistory(string path)
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
			const string LogPattern = "LOG_*.html";
			var files = Directory.GetFiles(path, LogPattern).Select(Path.GetFileName);
			
			var logName = files.FirstOrDefault(f => f.EndsWith($@"{Path.GetFileName(path)}.html"));
			if (!string.IsNullOrEmpty(logName))
			{
				return $@"{path}\{logName}";
			}
			this.logger.AddDetailMessage(Messages.GetNoLogFile(path));
			return string.Empty;
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
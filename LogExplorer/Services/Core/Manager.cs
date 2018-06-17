// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;
using LogExplorer.Services.OutputSystem;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Core
{
	public class Manager : IManager
	{
		#region Fields

		private readonly Logger logger;

		#endregion

		#region Constructors and Destructors

		public Manager()
		{
			this.LogOverview = new MvxObservableCollection<LogOverview>();
			this.logger = Logger.Instance;
		}

		#endregion

		#region Public Properties

		public MvxObservableCollection<LogOverview> LogOverview { get; set; }

		#endregion

		#region Public Methods and Operators

		public void DeleteLog(Log log)
		{
			var isDeleted = FileHelper.Delete(log.DirPath);
			if (!isDeleted)
			{
				return;
			}
			var overview = this.LogOverview.First(logs => logs.History.Contains(log));
			if (overview.History.Count > 1)
			{
				overview.History.Remove(log);
			}
			else
			{
				this.LogOverview.Remove(overview);
				var parent = FileHelper.GetParent(log.DirPath);
				if (FileHelper.EmptyDir(parent))
				{
					FileHelper.Delete(parent);
				}
			}
		}

		public void DeleteSelectedLogs()
		{
			var selectedLogs = this.GetSelectedLogs();
			var counter = 1;
			var selectedCount = selectedLogs.Count;

			foreach (var selectedLog in selectedLogs)
			{
				this.logger.AddMessage(Messages.GetDeletingCounter(counter, selectedCount));
				this.DeleteLog(selectedLog);
				counter++;
			}
		}

		public void Export(string exportPath, bool onlyLog = true)
		{
			var logs = this.GetSelectedLogs();
			if (logs == null
			    || !logs.Any())
			{
				this.logger.AddMessage(Messages.NothingSelected);
				return;
			}

			if (string.IsNullOrEmpty(exportPath))
			{
				this.logger.AddMessage(Messages.NoExportPath);
				return;
			}

			var time = DateTime.Now.ToString("yy_MM_dd_hh_mm_ss");
			var path = FileHelper.CombinePaths(exportPath, time);

			if (!FileHelper.DirExist(path))
			{
				if (!FileHelper.CreateDir(path))
				{
					return;
				}
			}

			foreach (var log in logs)
			{
				if (onlyLog)
				{
					this.CopyLog(log, path);
				}
				else
				{
					this.CopyDir(log, path);
				}
			}
			if (FileHelper.StartProcess(path))
			{
				this.logger.AddMessage(Messages.ExportSuccess);
			}
		}

		private void CopyDir(Log log, string destPath)
		{
			if (string.IsNullOrEmpty(log.DirPath))
			{
				this.logger.AddMessage(Messages.LogWithoutDirPath);
				return;
			}

			var dirCounter = 0;
			var newDirPath = FileHelper.CombinePaths(destPath, log.Name);
			while (FileHelper.DirExist(newDirPath))
			{
				dirCounter++;
				newDirPath = FileHelper.CombinePaths(destPath, $"{log.Name}-{dirCounter}");
			}
			FileHelper.CreateDir(newDirPath);

			var files = FileHelper.GetFiles(log.DirPath);
			foreach (var file in files)
			{
				var newFile = FileHelper.CombinePaths(newDirPath, FileHelper.GetFileName(file, true));
				FileHelper.CopyFile(file, newFile);
			}
		}

		private void CopyLog(Log log, string destPath)
		{
			if (string.IsNullOrEmpty(log.LogPath))
			{
				this.logger.AddMessage(Messages.GetNoLogFile(log.DirPath));
				return;
			}

			this.logger.AddDetailMessage(Messages.GetCopyingFile(log.Name, log.DirPath, destPath));
			var counter = 0;
			var newFilePath = FileHelper.CombinePaths(destPath, $"{log.Name}.html");

			while (FileHelper.FileExist(newFilePath))
			{
				counter++;
				newFilePath = FileHelper.CombinePaths(destPath, $"{log.Name}-{counter}.html");
			}

			FileHelper.CopyFile(log.LogPath, newFilePath);
		}

		#endregion

		#region Methods

		private List<Log> GetSelectedLogs()
		{
			var selectedLogs = this.LogOverview.SelectMany(l => l.History.Where(log => log.IsSelected)).ToList();
			return selectedLogs;
		}

		#endregion
	}
}
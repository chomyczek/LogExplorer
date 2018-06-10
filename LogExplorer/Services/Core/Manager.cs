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

		public void Delete(Log log)
		{
			
		}

		public void Export(string exportPath)
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

			if (!FileHelper.PathExist(path))
			{
				if (!FileHelper.CreateDir(path))
				{
					return;
				}
			}

			foreach (var log in logs)
			{
				if (string.IsNullOrEmpty(log.LogPath))
				{
					this.logger.AddMessage(Messages.GetNoLogFile(log.DirPath));
					continue;
				}

				this.logger.AddDetailMessage(Messages.GetCopyingFile(log.Name, log.DirPath, path));
				var counter = 0;
				var newFilePath = FileHelper.CombinePaths(path, $"{log.Name}.html");

				while (FileHelper.FileExist(newFilePath))
				{
					counter++;
					newFilePath = FileHelper.CombinePaths(path, $"{log.Name}-{counter}.html");
				}

				FileHelper.CopyFile(log.LogPath, newFilePath);
			}
			if (FileHelper.StartProcess(path))
			{
				this.logger.AddMessage(Messages.ExportSuccess);
			}
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
﻿// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Extensions;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;
using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Core
{
	public class Manager : IManager
	{
	    public Manager()
	    {
            LogOverview = new MvxObservableCollection<LogOverview>();

        }
		#region Public Properties

		public MvxObservableCollection<LogOverview> LogOverview { get; set; }

		#endregion

		#region Public Methods and Operators

		public void Export(List<Log> logs, string exportPath)
		{
			if (logs == null
			    || !logs.Any())
			{
				return;
			}

			if (string.IsNullOrEmpty(exportPath))
			{
				return;
			}

			var time = DateTime.Now.ToString("yy_MM_dd_hh_mm_ss");
			var path = FileHelper.CombinePaths(exportPath, time);

			if (!FileHelper.PathExist(path))
			{
				FileHelper.CreateDir(path);
			}

			foreach (var log in logs)
			{
				if (string.IsNullOrEmpty(log.LogPath))
				{
					//todo any message?
					continue;
				}
				var counter = 0;
				var newFilePath = FileHelper.CombinePaths(path, $"{log.Name}.html");

				while (FileHelper.FileExist(newFilePath))
				{
					counter++;
					newFilePath = FileHelper.CombinePaths(path, $"{log.Name}-{counter}.html");
				}

				FileHelper.CopyFile(log.LogPath, newFilePath);
				FileHelper.StartProcess(path);
			}
		}

		public List<Log> GetSelectedLogs()
		{
		    var selectedLogs = this.LogOverview.SelectMany(l => l.History.Where(log => log.IsSelected)).ToList();
            return selectedLogs;

		}

		#endregion
	}
}
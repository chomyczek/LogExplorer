// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;

using LogExplorer.Models;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Interfaces
{
	public interface IManager
	{
		#region Public Properties

		MvxObservableCollection<LogOverview> LogOverview { get; set; }

		#endregion

		#region Public Methods and Operators

		void Export(List<Log> logs, string exportPath);

		List<Log> GetSelectedLogs();

		#endregion
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

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

		void DeleteLog(Log log);

		void Export(string exportPath);

		#endregion
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogExplorer.Models;
using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Interfaces
{
	public interface IExplorer
	{
        #region Public Methods and Operators

        MvxObservableCollection<LogOverview> GetLogsRoot(string path);

		#endregion
	}
}
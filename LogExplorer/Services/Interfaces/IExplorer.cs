// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using LogExplorer.Models;
using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Services.Interfaces
{
	public interface IExplorer
	{
        #region Public Methods and Operators

		Task PopulateLogsRootAsync(MvxObservableCollection<LogOverview> logOverviews, string path, Action propertyChanges);

		#endregion
	}
}
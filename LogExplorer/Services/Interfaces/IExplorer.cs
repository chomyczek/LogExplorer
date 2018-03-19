// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;

using LogExplorer.Models;

#endregion

namespace LogExplorer.Services.Interfaces
{
	public interface IExplorer
	{
		#region Public Methods and Operators

		List<Log> GetLogsRoot(string path);

		#endregion
	}
}
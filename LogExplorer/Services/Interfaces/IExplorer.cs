// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

using System.Collections.Generic;

using LogExplorer.Models;

namespace LogExplorer.Services.Interfaces
{
	public interface IExplorer
	{
		List<Log> GetAllLogs();
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LogExplorer.Models;

#endregion

namespace LogExplorer.Services.Interfaces
{
	public interface ITester
	{
		#region Public Methods and Operators

		Task<bool> RerunAsync(Log log, Settings settings, CancellationToken ct);

		Task RerunQueueAwait(List<Log> logs, Settings settings, CancellationToken ct);

		#endregion
	}
}
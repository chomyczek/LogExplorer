using System.Collections.Generic;
using System.Threading.Tasks;

using LogExplorer.Models;

namespace LogExplorer.Services.Interfaces
{
    public interface ITester
    {

        Task<bool> RerunAsync(Log log, Settings settings);

	    void RerunQueue(List<Log> logs, Settings settings);
    }
}
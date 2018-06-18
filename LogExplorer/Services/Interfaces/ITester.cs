using System.Collections.Generic;

using LogExplorer.Models;

namespace LogExplorer.Services.Interfaces
{
    public interface ITester
    {

        void Rerun(Log log, Settings settings);

	    void RerunQueue(List<Log> logs, Settings settings);
    }
}
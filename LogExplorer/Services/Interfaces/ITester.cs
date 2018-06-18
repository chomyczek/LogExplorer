using LogExplorer.Models;

namespace LogExplorer.Services.Interfaces
{
    public interface ITester
    {

        void Rerun(Log log, Settings settings);

	    void RerunQueue();
    }
}
using LogExplorer.Models;

namespace LogExplorer.Services.Interfaces
{
    public interface ITester
    {

        void Rerun(Log name, Settings settings);
    }
}
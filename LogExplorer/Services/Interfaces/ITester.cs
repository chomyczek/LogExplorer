using LogExplorer.Models;

namespace LogExplorer.Services.Interfaces
{
    public interface ITester
    {

        void Rerun(string name, Settings settings);
    }
}
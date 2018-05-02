using System;
using System.IO;
using System.Windows;
using LogExplorer.Models;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;

namespace LogExplorer.Services.Core
{
    public class Tester : ITester
    {
        private const string LibSubDir = @"Packages\";
        private const string TesterName = @"Tester.exe";


        public void Rerun(string testName, Settings settings)
        {
            var librariesPath = FileHelper.CombinePaths(settings.TesterPath, LibSubDir);
            var pdbs = FileHelper.GetPdbs(librariesPath);

            var start = DateTime.Now;

            var component = GetCorrectComponent(pdbs, testName);

            var diff = DateTime.Now.Subtract(start);
            Console.WriteLine("Dll search took: {0}s", diff.TotalSeconds);

            if (string.IsNullOrEmpty(component))
            {
                MessageBox.Show(@"Correct component was not found.");
                return;
            }
            Console.WriteLine($@"Correct component was found = {component}.");
            var testerPath = FileHelper.CombinePaths(settings.TesterPath, TesterName);
            Execute(settings.TesterPath, testName, component);
        }

        private string GetCorrectComponent(string[] pdbs, string testName)
        {
            foreach (var pdb in pdbs)
            {
                try
                {
                    using (var sr = new StreamReader(pdb))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.IndexOf(testName, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                return FileHelper.GetFileName(pdb);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            return null;
        }

        private void Execute(string testerPath, string name, string component, string config = null)
        {
            if (!FileHelper.FileExist(FileHelper.CombinePaths(testerPath, TesterName)))
            {
                return;
            }
            var command = $"{TesterName} -p {component} -n {name}";
            FileHelper.StartSilentCmd(command, testerPath);
        }
    }
}
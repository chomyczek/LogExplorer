using System;
using System.IO;
using System.Linq;
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
        private const string ConfigPrefix = @"FinalConfiguration";


        public void Rerun(Log log, Settings settings)
        {
            var testName = log.Name;
            var librariesPath = FileHelper.CombinePaths(settings.TesterPath, LibSubDir);
            var pdbs = FileHelper.GetFiles(librariesPath, "pdb");

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

            var config = this.prepareConfig(settings, log);

            this.Execute(settings.TesterPath, testName, component, config , settings.IsHiddenTester);
        }

        private string prepareConfig(Settings settings, Log log)
        {
            switch (settings.ConfigMode)
            {
                case 1:
                    var xmls =FileHelper.GetFiles(log.DirPath, "xml");
                    return xmls.FirstOrDefault(file => FileHelper.GetFileName(file).StartsWith(ConfigPrefix));
                case 2:
                    return settings.CustomConfigPath;
                default:
                    return null;

            }
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

        private void Execute(string testerPath, string name, string component, string config, bool hidden)
        {
            if (!FileHelper.FileExist(FileHelper.CombinePaths(testerPath, TesterName)))
            {
                return;
            }
            string command = $"{TesterName} -p {component} -n {name}";
            if(FileHelper.FileExist(config))
            {
                command = $"{command} -c {config}";
            }
            else
            {
                Console.WriteLine($"Config file was incorrec ({config})");
            }            

            FileHelper.StartCmdProcess(command, testerPath, hidden);
        }
    }
}
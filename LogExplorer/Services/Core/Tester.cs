// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.IO;
using System.Linq;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;
using LogExplorer.Services.OutputSystem;

#endregion

namespace LogExplorer.Services.Core
{
	public class Tester : ITester
	{
		#region Constants

		private const string ConfigPrefix = @"FinalConfiguration";

		private const string LibSubDir = @"Packages\";

		private const string TesterName = @"Tester.exe";

		#endregion

		#region Fields

		private readonly Logger logger;

		#endregion

		#region Constructors and Destructors

		public Tester()
		{
			this.logger = Logger.Instance;
		}

		#endregion

		#region Public Methods and Operators

		public void Rerun(Log log, Settings settings)
		{
			var testName = log.Name;
			var librariesPath = FileHelper.CombinePaths(settings.TesterPath, LibSubDir);

			if (!FileHelper.DirExist(librariesPath))
			{
				Popup.ShowWarning(Messages.GetDllNotExist(librariesPath));
				return;
			}

			var pdbs = FileHelper.GetFiles(librariesPath, "pdb");
			var start = DateTime.Now;
			var component = this.GetCorrectComponent(pdbs, testName);
			var diff = DateTime.Now.Subtract(start);
			this.logger.AddMessage(Messages.GetDllSearch(diff.TotalSeconds));

			if (string.IsNullOrEmpty(component))
			{
				Popup.ShowWarning(Messages.DllNotFound);
				return;
			}

			this.logger.AddDetailMessage(Messages.GetDllFound(component));
			var config = this.PrepareConfig(settings, log);
			this.Execute(settings.TesterPath, testName, component, config, settings.IsHiddenTester);
		}

		public void RerunQueue()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Methods

		private void Execute(string testerPath, string name, string component, string config, bool hidden)
		{
			if (!FileHelper.FileExist(FileHelper.CombinePaths(testerPath, TesterName)))
			{
				Popup.ShowWarning(Messages.GetTesterNotFound(FileHelper.CombinePaths(testerPath, TesterName)));
				return;
			}
			string command = $"{TesterName} -p {component} -n {name}";
			if (FileHelper.FileExist(config))
			{
				command = $"{command} -c {config}";
			}
			else
			{
				this.logger.AddMessage(Messages.GetIncorrectConfigPath(config));
			}

			FileHelper.StartCmdProcess(command, testerPath, hidden);
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
					this.logger.AddMessage(Messages.GetReadFileException(e.Message));
				}
			}
			return null;
		}

		private string PrepareConfig(Settings settings, Log log)
		{
			switch (settings.ConfigMode)
			{
				//get config from history directory
				case 1:
					var xmls = FileHelper.GetFiles(log.DirPath, "xml");
					var xml = xmls.FirstOrDefault(file => FileHelper.GetFileName(file).StartsWith(ConfigPrefix));

					if (xml == null)
					{
						this.logger.AddMessage(Messages.GetConfigNotFound(log.DirPath));
					}

					return xml;
				//get config form custom path
				case 2:
					return settings.CustomConfigPath;
				//ignore config
				default:
					return null;
			}
		}

		#endregion
	}
}
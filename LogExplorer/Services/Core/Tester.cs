// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.Interfaces;
using LogExplorer.Services.OutputSystem;

using MvvmCross.Platform;

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
		private readonly IManager manager;
		private readonly IExplorer explorer;

		#endregion

		#region Constructors and Destructors

		public Tester(IExplorer explorer)
		{
			this.logger = Logger.Instance;
			this.explorer = explorer;
			this.manager = Mvx.Resolve<Manager>();
		}

		#endregion

		#region Public Methods and Operators

		public async Task<bool> RerunAsync(Log log, Settings settings)
		{
			var testName = log.Name;
			var librariesPath = FileHelper.CombinePaths(settings.TesterPath, LibSubDir);

			if (!FileHelper.DirExist(librariesPath))
			{
				Popup.ShowWarning(Messages.GetDllNotExist(librariesPath));
				return await Task.Run(() => false);
			}

			var pdbs = FileHelper.GetFiles(librariesPath, "pdb");
			var start = DateTime.Now;
			var component = this.GetCorrectComponent(pdbs, testName);
			var diff = DateTime.Now.Subtract(start);
			this.logger.AddMessage(Messages.GetDllSearch(diff.TotalSeconds));

			if (string.IsNullOrEmpty(component))
			{
				Popup.ShowWarning(Messages.DllNotFound);
				return await Task.Run(() => false);
			}

			this.logger.AddDetailMessage(Messages.GetDllFound(component));
			var config = this.PrepareConfig(settings, log);
			return await this.ExecuteAsync(settings.TesterPath, testName, component, config, settings.IsHiddenTester);
		}

		public async void RerunQueue(List<Log> logs, Settings settings)
		{
			var counter = 1;
			var count = logs.Count;
			foreach (var log in logs)
			{
				this.logger.AddMessage(Messages.GetRunningCounter(log.Name, counter, count));
				if (await this.RerunAsync(log, settings))
				{
					var updatedHistory = this.explorer.GetLogHistory(FileHelper.GetParent(log.DirPath));
					this.manager.UpdateOverview(updatedHistory);
				}
				this.logger.AddMessage(Messages.GetExecutionEnded(log.Name));
				counter++;
			}
		}

		#endregion

		#region Methods

		private async Task<bool> ExecuteAsync(string testerPath, string name, string component, string config, bool hidden)
		{
			var testerExePath = FileHelper.CombinePaths(testerPath, TesterName);
			if (!FileHelper.FileExist(testerExePath))
			{
				Popup.ShowWarning(Messages.GetTesterNotFound(FileHelper.CombinePaths(testerPath, TesterName)));
				return await Task.Run(()=>  false);
			}
			string paramters = $"-p {component} -n {name}";
			if (FileHelper.FileExist(config))
			{
				paramters = $"{paramters} -c {config}";
			}
			else if (!string.IsNullOrEmpty(config))
			{
				this.logger.AddMessage(Messages.GetIncorrectConfigPath(config));
			}

			return await Task.Run(() => FileHelper.StartProcessWithArguments(testerExePath, paramters, hidden));
		}

		private string GetCorrectComponent(string[] pdbs, string testName)
		{
#if DEBUG
			return "FakeComponnet";
#endif
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
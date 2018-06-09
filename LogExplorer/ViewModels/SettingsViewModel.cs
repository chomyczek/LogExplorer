// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;

using LogExplorer.Models;
using LogExplorer.Services.Core;
using LogExplorer.Services.Helpers;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

#endregion

namespace LogExplorer.ViewModels
{
	public class SettingsViewModel : MvxViewModel
	{
		#region Fields

		private MvxCommand cmdPickCustomConfig;

		private Tuple<int, string> configSetting;

		#endregion

		#region Constructors and Destructors

		public SettingsViewModel()
		{
			this.settings = Mvx.Resolve<Repository>().Settings;
			this.ConfigSettingDictionary = new List<Tuple<int, string>>
			                               {
				                               new Tuple<int, string>(0, "Tester default"),
				                               new Tuple<int, string>(1, "Previously executed"),
				                               new Tuple<int, string>(2, "Custom")
			                               };
			this.cmdPickCustomConfig =
				new MvxCommand(
					() => { this.CustomConfigPath = FileHelper.SelectFile(this.CustomConfigPath, "xml", "Config file"); },
					() => this.IsConfigPathEnabled);
			this.ConfigSetting = this.ConfigSettingDictionary[this.settings.ConfigMode];
		}

		#endregion

		#region Public Properties

		public IMvxCommand CmdCancel
		{
			get
			{
				return new MvxCommand(() => { this.Close(this); });
			}
		}

		/// <summary>
		/// Require private MvxCommand field to  resolve Enable function.
		/// </summary>
		public IMvxCommand CmdPickCustomConfig => this.cmdPickCustomConfig;

		public IMvxCommand CmdPickExportDir
		{
			get
			{
				return new MvxCommand(() => { this.ExportPath = FileHelper.SelectDir(this.ExportPath); });
			}
		}

		public IMvxCommand CmdPickRootDir
		{
			get
			{
				return new MvxCommand(() => { this.RootLogsPath = FileHelper.SelectDir(this.RootLogsPath); });
			}
		}

		public IMvxCommand CmdPickTesterDir
		{
			get
			{
				return new MvxCommand(() => { this.TesterPath = FileHelper.SelectDir(this.ExportPath); });
			}
		}

		public IMvxCommand CmdSave
		{
			get
			{
				return new MvxCommand(
					() =>
					{
						Mvx.Resolve<Repository>().UpdateSettings();
						this.Close(this);
					});
			}
		}

		public Tuple<int, string> ConfigSetting
		{
			get
			{
				return this.configSetting;
			}
			set
			{
				this.configSetting = value;
				this.settings.ConfigMode = value.Item1;
				this.IsConfigPathEnabled = value.Item1 == 2;
				this.CmdPickCustomConfig.RaiseCanExecuteChanged();
				this.RaisePropertyChanged(() => this.ConfigSetting);
			}
		}

		public List<Tuple<int, string>> ConfigSettingDictionary { get; }

		public string CustomConfigPath
		{
			get
			{
				return this.settings.CustomConfigPath;
			}
			set
			{
				this.settings.CustomConfigPath = value;
				this.RaisePropertyChanged(() => this.CustomConfigPath);
			}
		}

		public string ExportPath
		{
			get
			{
				return this.settings.ExportPath;
			}
			set
			{
				this.settings.ExportPath = value;
				this.RaisePropertyChanged(() => this.ExportPath);
			}
		}

		public bool IsHiddenTester
		{
			get
			{
				return this.settings.IsHiddenTester;
			}
			set
			{
				this.settings.IsHiddenTester = value;
				this.RaisePropertyChanged(() => this.IsHiddenTester);
			}
		}

		public bool IsLoggerShowDetails
		{
			get
			{
				return this.settings.IsLoggerShowDetails;
			}
			set
			{
				this.settings.IsLoggerShowDetails = value;
				this.RaisePropertyChanged(() => this.IsLoggerShowDetails);
			}
		}

		public string LoggerMemory
		{
			get
			{
				return this.settings.LoggerMemory.ToString();
			}
			set
			{
				int result;
				if (!int.TryParse(value, out result))
				{
					result = 0;
				}
				this.settings.LoggerMemory = result;
				this.RaisePropertyChanged(() => this.LoggerMemory);
			}
		}

		public string RootLogsPath
		{
			get
			{
				return this.settings.RootLogsPath;
			}
			set
			{
				this.settings.RootLogsPath = value;
				this.RaisePropertyChanged(() => this.RootLogsPath);
			}
		}

		public string TesterPath
		{
			get
			{
				return this.settings.TesterPath;
			}
			set
			{
				this.settings.TesterPath = value;
				this.RaisePropertyChanged(() => this.TesterPath);
			}
		}

		#endregion

		#region Properties

		private bool IsConfigPathEnabled { get; set; }

		private Settings settings { get; }

		#endregion
	}
}
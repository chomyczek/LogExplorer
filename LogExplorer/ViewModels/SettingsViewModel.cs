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
        #region Properties

        private Settings settings { get; }

        #endregion

        #region Public Properties

        public IMvxCommand CmdCancel
        {
            get { return new MvxCommand(() => { Close(this); }); }
        }

        public IMvxCommand CmdPickExportDir
        {
            get { return new MvxCommand(() => { ExportPath = FileHelper.SelectDir(ExportPath); }); }
        }

        public IMvxCommand CmdPickTesterDir
        {
            get { return new MvxCommand(() => { TesterPath = FileHelper.SelectDir(ExportPath); }); }
        }

        public IMvxCommand CmdPickRootDir
        {
            get { return new MvxCommand(() => { RootLogsPath = FileHelper.SelectDir(RootLogsPath); }); }
        }

        public IMvxCommand CmdPickCustomConfig
        {
            get { return new MvxCommand(() => { CustomConfigPath = FileHelper.SelectFile(CustomConfigPath,"xml","Config file"); }, () => IsConfigPathEnabled); }
        }

        public IMvxCommand CmdSave
        {
            get
            {
                return new MvxCommand(
                    () =>
                    {
                        Mvx.Resolve<Repository>().UpdateSettings(settings);
                        Close(this);
                    });
            }
        }

        public List<Tuple<int, string>> ConfigSettingDictionary { get; }

        public string ExportPath
        {
            get { return settings.ExportPath; }
            set
            {
                settings.ExportPath = value;
                RaisePropertyChanged(() => ExportPath);
            }
        }

        public string TesterPath
        {
            get { return settings.TesterPath; }
            set
            {
                settings.TesterPath = value;
                RaisePropertyChanged(() => TesterPath);
            }
        }

        public string RootLogsPath
        {
            get { return settings.RootLogsPath; }
            set
            {
                settings.RootLogsPath = value;
                RaisePropertyChanged(() => RootLogsPath);
            }
        }

        public string CustomConfigPath
        {
            get { return settings.CustomConfigPath; }
            set
            {
                settings.CustomConfigPath = value;
                RaisePropertyChanged(() => CustomConfigPath);
            }
        }

        #endregion

        #region Public Methods and Operators

        public SettingsViewModel()
        {
            settings = Mvx.Resolve<Repository>().GetSettings();
            ConfigSettingDictionary = new List<Tuple<int, string>>
            {
                new Tuple<int, string>(0, "Tester default"),
                new Tuple<int, string>(1, "Previously executed"),
                new Tuple<int, string>(2, "Custom")
            };
            ConfigSetting = ConfigSettingDictionary[settings.ConfigMode];
        }

        public Tuple<int, string> ConfigSetting
        {
            get { return configSetting; }
            set
            {
                configSetting = value;
                settings.ConfigMode = value.Item1;
                IsConfigPathEnabled = value.Item1 == 2;
                RaisePropertyChanged(() => ConfigSetting);
            }
        }

        public bool IsConfigPathEnabled
        {
            get { return isConfigPathEnabled; }
            set
            {
                isConfigPathEnabled = value;
                RaisePropertyChanged(() => IsConfigPathEnabled);
            }
        }


        private Tuple<int, string> configSetting;
        private bool isConfigPathEnabled;

        #endregion
    }
}
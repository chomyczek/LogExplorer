// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

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
		#region Public Properties

		public IMvxCommand CmdCancel
		{
			get
			{
				return new MvxCommand(() => { this.Close(this); });
			}
		}

		public IMvxCommand CmdPickExportDir
		{
			get
			{
				return new MvxCommand(() => { ExportPath = FileHelper.SelectDir(ExportPath); });
			}
		}

        public IMvxCommand CmdPickTesterDir
        {
            get
            {
                return new MvxCommand(() => { TesterPath = FileHelper.SelectDir(ExportPath); });
            }
        }

        public IMvxCommand CmdPickRootDir
		{
			get
			{
				return new MvxCommand(() => { RootLogsPath = FileHelper.SelectDir(RootLogsPath); });
			}
		}

		public IMvxCommand CmdSave
		{
			get
			{
				return new MvxCommand(
					() =>
					{
						Mvx.Resolve<Repository>().UpdateSettings(this.settings);
						this.Close(this);
					});
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

		#endregion

		#region Properties

		private Settings settings { get; set; }

		#endregion

		#region Public Methods and Operators

		public override void Start()
		{
			this.settings = Mvx.Resolve<Repository>().GetSettings();
			base.Start();
		}

		#endregion
	}
}
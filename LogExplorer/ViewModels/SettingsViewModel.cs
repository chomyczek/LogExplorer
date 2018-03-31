// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using LogExplorer.Models;
using LogExplorer.Services.Core;

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

		public string RootLogsPath
		{
			get
			{
				return this.settings.RootLogsPath;
			}
			set
			{
				this.settings.RootLogsPath = value;
                this.RaisePropertyChanged(()=>this.RootLogsPath);
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
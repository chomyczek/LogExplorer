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
		private Settings settings { get; set; }
		public string LogPath { get
		{
			return this.settings.LogsPath;
		}
			set
			{
				this.settings.LogsPath = value;
			}
		}

		public override void Start()
		{
			this.settings = Mvx.Resolve<Repository>().GetSettings();
			base.Start();
		}

		public IMvxCommand CmdCancel
		{
			get
			{
				return new MvxCommand(
					() =>
					{
						this.Close(this);
					});
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
	}
}
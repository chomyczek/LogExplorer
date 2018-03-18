// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.ViewModels
{
	public class SettingsViewModel : MvxViewModel
	{
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
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Windows;

using LogExplorer.Customs;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

#endregion

namespace LogExplorer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		#region Fields

		// Flag to determine whether setup has been performed already.
		private bool setupComplete;

		#endregion

		#region Methods

		protected override void OnActivated(EventArgs e)
		{
			if (!this.setupComplete)
			{
				this.DoSetup();
			}
			base.OnActivated(e);
		}

		private void DoSetup()
		{
			var presenter = new MultiRegionPresenter(this.MainWindow);

			var setup = new Setup(this.Dispatcher, presenter);
			setup.Initialize();

			var start = Mvx.Resolve<IMvxAppStart>();
			start.Start();

			this.setupComplete = true;
		}

		#endregion
	}
}
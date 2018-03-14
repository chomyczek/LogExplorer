using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using LogExplorer.Customs;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace LogExplorer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		// Flag to determine whether setup has been performed already.
		private bool setupComplete;

		protected override void OnActivated(EventArgs e)
		{
			if (!this.setupComplete) this.DoSetup();
			base.OnActivated(e);
		}

		private void DoSetup()
		{
			var presenter = new CustomViewPresenter(this.MainWindow);

			var setup = new Setup(this.Dispatcher, presenter);
			setup.Initialize();

			var start = Mvx.Resolve<IMvxAppStart>();
			start.Start();

			this.setupComplete = true;
		}
	}
}

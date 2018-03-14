using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogExplorer.Services.Core;
using LogExplorer.Services.Interfaces;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace LogExplorer.Customs
{
	public class App : MvxApplication
	{
		/// <summary>
		/// Setup IoC registrations.
		/// </summary>
		public App()
		{
			Mvx.RegisterType<IExplorer, Explorer>();

			// Tells the MvvmCross framework that whenever any code requests an IMvxAppStart reference,
			// the framework should return that same appStart instance.
			var appStart = new CustomAppStart();
			Mvx.RegisterSingleton<IMvxAppStart>(appStart);
			
		}
	}
}

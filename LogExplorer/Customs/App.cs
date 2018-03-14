// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using LogExplorer.Services.Core;
using LogExplorer.Services.Interfaces;

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

#endregion

namespace LogExplorer.Customs
{
	public class App : MvxApplication
	{
		#region Constructors and Destructors

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

		#endregion
	}
}
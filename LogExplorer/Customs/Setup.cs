// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Windows.Threading;

using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Platform;
using MvvmCross.Wpf.Views.Presenters;

#endregion

namespace LogExplorer.Customs
{
	public class Setup : MvxWpfSetup
	{
		#region Constructors and Destructors

		public Setup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
			: base(uiThreadDispatcher, presenter)
		{
		}

		#endregion

		#region Methods

		protected override IMvxApplication CreateApp()
		{
			return new App();
		}

		#endregion
	}
}
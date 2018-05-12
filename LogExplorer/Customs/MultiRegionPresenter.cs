// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Linq;
using System.Windows;

using MvvmCross.Wpf.Views.Presenters;

#endregion

namespace LogExplorer.Customs
{
	public class MultiRegionPresenter :  MvxSimpleWpfViewPresenter
	{
		#region Fields

		private readonly MainWindow mainWindow;

		#endregion

		#region Constructors and Destructors

		public MultiRegionPresenter(Window mainWindow)
			: base(null)
		{
			this.mainWindow = mainWindow as MainWindow;
		}

		#endregion

		#region Public Methods and Operators

		
		public override void Present(FrameworkElement frameworkElement)
		{
			// this is really hacky - do it using attributes isnt
			var attribute =
				frameworkElement.GetType().GetCustomAttributes(typeof(RegionAttribute), true).FirstOrDefault() as RegionAttribute;

			var regionName = attribute == null ? null : attribute.Name;
			this.mainWindow.PresentInRegion(frameworkElement, regionName);
		}

		#endregion
	}
}
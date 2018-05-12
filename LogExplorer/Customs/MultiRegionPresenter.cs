// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Views.Presenters;

#endregion

namespace LogExplorer.Customs
{
	public class MultiRegionPresenter :  MvxSimpleWpfViewPresenter
	{
		#region Fields

		private readonly MainWindow mainWindow;

		// Simple stack for storing the navigation tree
		private Stack<FrameworkElement> navigationTree = new Stack<FrameworkElement>();

		#endregion

		#region Constructors and Destructors

		public MultiRegionPresenter(ContentControl contentControl) : base(contentControl)
		{
			var window = contentControl as MainWindow;
			if (window != null)
			{
				this.mainWindow = window;
			}
		}

		#endregion

		#region Public Methods and Operators
		public override void ChangePresentation(MvxPresentationHint hint)
		{
			// This will be null if hint isn't a close hint.
			var closeHint = hint as MvxClosePresentationHint;

			// Only navigate up the tree if the hint is an MvxClosePresentationHint
			// and there is a parent view in the navigation tree.
			if (closeHint != null
				&& this.navigationTree.Count > 1)
			{
				this.navigationTree.Pop();
				this.Present(this.navigationTree.Peek());
				base.ChangePresentation(hint);
			}
		}

		public override void Present(FrameworkElement frameworkElement)
		{
			// this is really hacky - do it using attributes isnt
			var attribute =
				frameworkElement.GetType().GetCustomAttributes(typeof(RegionAttribute), true).FirstOrDefault() as RegionAttribute;

			var regionName = attribute?.Name;
			this.mainWindow.PresentInRegion(frameworkElement, regionName, ref this.navigationTree);
		}

		#endregion
	}
}
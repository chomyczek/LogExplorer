// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Views.Presenters;

#endregion

namespace LogExplorer.Customs
{
	public class CustomViewPresenter : MvxSimpleWpfViewPresenter, IMvxWpfViewPresenter
	{
		#region Fields

		// Simple stack for storing the navigation tree
		private Stack<FrameworkElement> navigationTree = new Stack<FrameworkElement>();

		#endregion

		#region Constructors and Destructors

		public CustomViewPresenter(ContentControl contentControl)
			: base(contentControl)
		{
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
			this.navigationTree.Push(frameworkElement);
			base.Present(frameworkElement);
		}

		#endregion
	}
}
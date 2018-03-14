// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using LogExplorer.ViewModels;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.Customs
{
	public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
	{
		#region Public Methods and Operators

		/// <summary>
		/// Hint can take command-line startup parameters, and then pass them to the called view models.
		/// </summary>
		/// <param name="hint"></param>
		public void Start(object hint = null)
		{
			// ShowViewModel is a core navigation mechanism in MvvmCross.
			// for now, just start the regular RootViewModel view.
			this.ShowViewModel<RootViewModel>();
		}

		#endregion
	}
}
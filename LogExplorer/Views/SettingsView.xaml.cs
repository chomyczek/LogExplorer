// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using LogExplorer.Customs;

using MvvmCross.Wpf.Views;

#endregion

namespace LogExplorer.Views
{
	/// <summary>
	/// Interaction logic for SettingsView.xaml
	/// </summary>
	[Region("Detail")]
	public partial class SettingsView : MvxWpfView
	{
		#region Constructors and Destructors

		public SettingsView()
		{
			InitializeComponent();
		}

		#endregion
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Windows;

#endregion

namespace LogExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructors and Destructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		#region Public Methods and Operators

		public void PresentInRegion(
			FrameworkElement frameworkElement,
			string regionName,
			ref Stack<FrameworkElement> navigationTree)
		{
			switch (regionName)
			{
				case "Logger":
					this.LoggerRow.Children.Clear();
					this.LoggerRow.Children.Add(frameworkElement);
					break;
				default:
					navigationTree.Push(frameworkElement);
					this.MainViewRow.Children.Clear();
					this.MainViewRow.Children.Add(frameworkElement);

					break;
			}
		}

		#endregion
	}
}
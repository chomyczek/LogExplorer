// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Windows;

#endregion

namespace LogExplorer.Services.OutputSystem
{
	public class Popup
	{
		#region Public Methods and Operators

		public static void ShowError(string message)
		{
			MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public static void ShowInfo(string message)
		{
			MessageBox.Show(message, "Info!", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public static void ShowWarning(string message)
		{
			MessageBox.Show(message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		#endregion
	}
}
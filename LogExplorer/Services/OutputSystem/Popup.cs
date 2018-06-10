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
			MessageBox.Show(message, Messages.HeaderError, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public static void ShowInfo(string message)
		{
			MessageBox.Show(message, Messages.HeaderInfo, MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public static void ShowWarning(string message)
		{
			MessageBox.Show(message, Messages.HeaderWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		public static bool ShowConfirm(string message)
		{
			if (MessageBox.Show(message, Messages.HeaderWarning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				return true;
			}
			return false;
		}

		#endregion
	}
}
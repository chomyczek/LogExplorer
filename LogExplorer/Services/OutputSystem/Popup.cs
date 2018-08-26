// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Threading.Tasks;

using LogExplorer.Models;

using MaterialDesignThemes.Wpf;

#endregion

namespace LogExplorer.Services.OutputSystem
{
	public class Popup
	{
		#region Public Methods and Operators

		public static bool ShowConfirm(string message)
		{
			var result = ShowConfirmAsync(message);
			return result.ToString().Equals("True");
		}

		public static void ShowError(string message)
		{
			var dialog = new WarningDialog() { Header = Messages.HeaderWarning, Content = message };
			DialogHost.Show(dialog);
		}

		public static void ShowWarning(string message)
		{
			var dialog = new WarningDialog() { Header = Messages.HeaderError, Content = message };
			DialogHost.Show(dialog);
		}

		#endregion

		#region Methods

		private static async Task<string> ShowConfirmAsync(string message)
		{
			var dialog = new ConfirmDialog() { Header = Messages.HeaderWarning, Content = message };
			var task = await DialogHost.Show(dialog);

			return task.ToString();
		}

		#endregion
	}
}
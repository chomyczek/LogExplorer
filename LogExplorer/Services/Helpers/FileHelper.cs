// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings



#endregion

namespace LogExplorer.Services.Helpers
{
	public class FileHelper
	{
		#region Public Methods and Operators

		public static void OpenLog(string path)
		{
			//todo try catch
			System.Diagnostics.Process.Start(path);
		}

		#endregion
	}
}
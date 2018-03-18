// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings



#endregion

using System.Diagnostics;

namespace LogExplorer.Services.Helpers
{
	public class FileHelper
	{
		#region Public Methods and Operators

		public static void StartProcess(string path)
		{
			//todo try catch
			Process.Start(path);
		}

		#endregion
	}
}
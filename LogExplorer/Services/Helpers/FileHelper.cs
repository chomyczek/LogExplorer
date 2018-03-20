// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings



#endregion

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LogExplorer.Services.Helpers
{
	public static class FileHelper
	{
		#region Public Methods and Operators

		public static void StartProcess(string path)
		{
			//todo try catch
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			Process.Start(path);
		}

		#endregion

		public static bool FileExist(string path)
		{
			return File.Exists(path);
		}

		public static string GetLocalPath(string fileName)
		{
			var assembly = Assembly.GetEntryAssembly();
			var titleAttribute = (AssemblyTitleAttribute)assembly.GetCustomAttribute(typeof(AssemblyTitleAttribute));
			var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var fullPath = Path.Combine(localAppDataPath, titleAttribute.Title);
			Directory.CreateDirectory(fullPath);

			return Path.Combine(fullPath, fileName);
		}
	}
}
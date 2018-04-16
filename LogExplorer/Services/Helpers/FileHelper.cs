// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

#endregion

#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace LogExplorer.Services.Helpers
{
	public static class FileHelper
	{
		#region Public Methods and Operators

		public static string CombinePaths(string p1, string p2)
		{
			//todo
			string combination;
			try
			{
				combination = Path.Combine(p1, p2);
			}
			catch (Exception e)
			{
				if (p1 == null)
				{
					p1 = "null";
				}
				if (p2 == null)
				{
					p2 = "null";
				}
				Console.WriteLine("You cannot combine '{0}' and '{1}' because: {2}{3}", p1, p2, Environment.NewLine, e.Message);
				return p1;
			}
			return combination;
		}

		public static void CopyFile(string source, string destination)
		{
			File.Copy(source, destination, true);
		}

		public static void CreateDir(string path)
		{
			//todo check if directory can be created
			Directory.CreateDirectory(path);
		}

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

		public static bool PathExist(string path)
		{
			return Directory.Exists(path);
		}

		public static string SelectDir(string path)
		{
			var directory = path;
			using (var dialog = new FolderBrowserDialog())
			{
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					directory = dialog.SelectedPath;
				}
			}
			return directory;
		}

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
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

#endregion

#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using LogExplorer.Services.OutputSystem;

#endregion

namespace LogExplorer.Services.Helpers
{
	public static class FileHelper
	{
		private static readonly Logger Logger = Logger.Instance;
		#region Public Methods and Operators

		public static string CombinePaths(string p1, string p2)
		{
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
				Logger.AddDetailMessage(Messages.GetCombineProblem(p1,p2,e.Message));
				
				return p1;
			}
			return combination;
		}

		public static bool Delete(string path)
		{
			if (!PathExist(path))
			{
				return false;
			}
			var files = GetFiles(path);
			foreach (var file in files)
			{
				File.Delete(file);
			}
			files = GetFiles(path);
			if (files.Any())
			{
				Logger.AddMessage(Messages.GetNotallFilesDeleted(files));
				return false;
			}
			try
			{
				Directory.Delete(path);
			}
			catch (Exception e)
			{
				Logger.AddMessage(Messages.GetDeletDirException(e.Message));
				return false;
			}
			Logger.AddMessage(Messages.GetDeleteSuccess(path));
			return true;
		}

	    public static string[] GetFiles(string path, string extension = "*")
	    {
	        if (!PathExist(path))
	        {
	            return new string[0];
	        }

            var files = Directory.GetFiles(path, $"*.{extension}");

	        return files;
	    }
		
        public static string GetFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static void CopyFile(string source, string destination)
		{
			File.Copy(source, destination, true);
		}

		public static bool CreateDir(string path)
		{
			try
			{
				Directory.CreateDirectory(path);
			}
			catch (Exception e)
			{
				Popup.ShowError(Messages.GetCantCreateDir(e.Message));
				return false;
			}
			Logger.AddDetailMessage(Messages.GetCreateDirSuccess(path));
			return true;
		}
        
        public static bool FileExist(string path)
		{
	        if (!string.IsNullOrEmpty(path)
	            && File.Exists(path))
	        {
		        return true;
	        }

			Logger.AddDetailMessage(Messages.GetFileNotExist(path));
	        return false;
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
			if (!string.IsNullOrEmpty(path)
				&& Directory.Exists(path))
			{
				return true;
			}

			Logger.AddDetailMessage(Messages.GetDirNotExist(path));
			return false;
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

	    public static string SelectFile(string path, string extension, string extensionDescription)
	    {
	        var newPath = path;

	        using (
	            var openFileDialog = new OpenFileDialog
	            {
	                InitialDirectory =
	                    string.IsNullOrEmpty(path) ? @"c:\" : Path.GetDirectoryName(path),
	                Filter = $"{extensionDescription} (*.{extension})|*.{extension}",
	                RestoreDirectory = true,
	                Multiselect = false
	            })
	        {
	            if (openFileDialog.ShowDialog() == DialogResult.OK)
	            {
	                newPath = openFileDialog.FileName;
	            }
	        }

	        return newPath;
	    }

        public static bool StartProcess(string path)
		{
			var attr = File.GetAttributes(path);
			
	        if ((attr & FileAttributes.Directory) == FileAttributes.Directory
	            && !PathExist(path))
	        {
				return false;
	        }
	        if (!FileExist(path))
	        {
		        return false;
	        }

	        var process = new Process { StartInfo = new ProcessStartInfo(path) };
			
			try
	        {
		        if (process.Start())
		        {
			        return true;
		        }
				Popup.ShowWarning(Messages.GetProcessNotStart(path));
				return false;

	        }
	        catch (Exception e)
	        {
				Popup.ShowError(Messages.GetProcessException(path, e.Message));
		        return false;
	        }
			
		}

	    public static bool StartCmdProcess(string command, string workingDir, bool hidden)
	    {
	        if (string.IsNullOrEmpty(command))
	        {
	            return false;
	        }

	        var process = new Process();
	        var startInfo = new ProcessStartInfo
	        {
	            WindowStyle = hidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
	            WorkingDirectory = workingDir,
	            FileName = "cmd.exe",
	            Arguments = $@"/C {command}"
	        };
	        process.StartInfo = startInfo;

			try
			{
				if (process.Start())
				{
					return true;
				}
				Popup.ShowWarning(Messages.GetProcessNotStart(command));
				return false;

			}
			catch (Exception e)
			{
				Popup.ShowError(Messages.GetProcessException(command, e.Message));
				return false;
			}
			
	    }

        #endregion
    }
}
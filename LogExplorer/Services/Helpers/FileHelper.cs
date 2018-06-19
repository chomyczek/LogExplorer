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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using LogExplorer.Services.OutputSystem;

#endregion

namespace LogExplorer.Services.Helpers
{
	public static class FileHelper
	{
		#region Static Fields

		private static readonly Logger Logger = Logger.Instance;

		#endregion

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
				Logger.AddDetailMessage(Messages.GetCombineProblem(p1, p2, e.Message));

				return p1;
			}
			return combination;
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

		public static bool Delete(string path)
		{
			if (!DirExist(path))
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
				if (DirExist(path))
				{
					StartProcess(path);
				}
				Logger.AddMessage(Messages.GetDeletDirException(e.Message));
				return false;
			}
			Logger.AddMessage(Messages.GetDeleteSuccess(path));
			return true;
		}

		public static bool DirExist(string path)
		{
			if (!string.IsNullOrEmpty(path)
			    && Directory.Exists(path))
			{
				return true;
			}

			Logger.AddDetailMessage(Messages.GetDirNotExist(path));
			return false;
		}

		public static bool EmptyDir(string path)
		{
			var containsFiles = GetFiles(path).Any();
			var containsDirs = Directory.GetDirectories(path).Any();
			return !containsDirs && !containsFiles;
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

		public static string GetFileName(string path, bool withExtension = false)
		{
			if (withExtension)
			{
				return Path.GetFileName(path);
			}

			return Path.GetFileNameWithoutExtension(path);
		}

		public static string[] GetFiles(string path, string extension = "*")
		{
			if (!DirExist(path))
			{
				return new string[0];
			}

			var files = Directory.GetFiles(path, $"*.{extension}");

			return files;
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

		public static string GetParent(string path)
		{
			var parent = Directory.GetParent(path);
			return parent.FullName;
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
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			var attr = File.GetAttributes(path);

			if (attr.HasFlag(FileAttributes.Directory))
			{
				if (!DirExist(path))
				{
					return false;
				}
			}
			else
			{
				if (!FileExist(path))
				{
					return false;
				}
			}

			var process = new Process { StartInfo = new ProcessStartInfo(path) };

			try
			{
				process.Start();
				return true;
			}
			catch (Exception e)
			{
				Popup.ShowError(Messages.GetProcessException(path, e.Message));
				return false;
			}
		}

		public static async Task<bool> StartProcessWithArguments(
			string filePath,
			string arguments,
			bool hidden,
			CancellationToken ct)
		{
			if (string.IsNullOrEmpty(arguments))
			{
				return false;
			}

			var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = hidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                WorkingDirectory = Path.GetDirectoryName(filePath),
                FileName = "cmd.exe",
                Arguments = $@"/C {GetFileName(filePath, true)} {arguments}"
            };
            //todo fix this way
            //var startInfo = new ProcessStartInfo
            //{
            //    WindowStyle = hidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
            //    FileName = filePath,
            //    Arguments = arguments
            //       };
            process.StartInfo = startInfo;
			var processStr = $"'{GetFileName(filePath, true)} {arguments}'";
			try
			{
				if (process.Start())
				{
					await process.WaitForExitAsync(ct);
					return true;
				}
				Popup.ShowWarning(Messages.GetProcessNotStart(processStr));
				return false;
			}
			catch (OperationCanceledException)
			{
#region todo delete when fixed 
                var processlist = Process.GetProcesses().Where(p=>p.ProcessName.Contains(GetFileName(filePath)));

                foreach (var coProcess in processlist)
			    {
			        coProcess.Kill();
			    }
#endregion

                process.Kill();
				Logger.AddMessage(Messages.KillingProccess);
				throw;
			}
			catch (Exception e)
			{
				Popup.ShowError(Messages.GetProcessException(processStr, e.Message));
				return false;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Waits asynchronously for the process to exit.
		/// </summary>
		/// <param name="process">The process to wait for cancellation.</param>
		/// <param name="cancellationToken">
		/// A cancellation token. If invoked, the task will return
		/// immediately as canceled.
		/// </param>
		/// <returns>A Task representing waiting for the process to end.</returns>
		private static Task WaitForExitAsync(
			this Process process,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var tcs = new TaskCompletionSource<object>();
			process.EnableRaisingEvents = true;
			process.Exited += (sender, args) => tcs.TrySetResult(null);
			if (cancellationToken != default(CancellationToken))
			{
				cancellationToken.Register(tcs.SetCanceled);
			}

			return tcs.Task;
		}

		#endregion
	}
}
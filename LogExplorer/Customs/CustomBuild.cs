// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Globalization;
using System.Reflection;

using LogExplorer.Services.OutputSystem;

#endregion

namespace LogExplorer.Customs
{
	/// <summary>
	/// New entry point class.
	/// This class is required for combining multiple assemblies into a single EXE for a WPF application.
	/// http://www.digitallycreated.net/Blog/61/combining-multiple-assemblies-into-a-single-exe-for-a-wpf-application
	/// </summary>
	public class CustomBuild
	{
		#region Public Methods and Operators

		[STAThread]
		public static void Main()
		{
			AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

			try
			{
				LogExplorer.App.Main();
			}
			catch (Exception e)
			{
				Popup.ShowError(Messages.GetUncatchedException(e.Message));
				throw;
			}
			
		}

		#endregion

		#region Methods

		private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			var assemblyName = new AssemblyName(args.Name);

			var path = $"{assemblyName.Name}.dll";
			if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
			{
				path = $@"{assemblyName.CultureInfo}\{path}";
			}

			using (var stream = executingAssembly.GetManifestResourceStream(path))
			{
				if (stream == null)
				{
					return null;
				}

				var assemblyRawBytes = new byte[stream.Length];
				stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
				return Assembly.Load(assemblyRawBytes);
			}
		}

		#endregion
	}
}
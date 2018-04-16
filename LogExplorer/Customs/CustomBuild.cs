// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

#endregion

#region Usings

using System;
using System.Globalization;
using System.Reflection;

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
			LogExplorer.App.Main();
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
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

namespace LogExplorer.Services.OutputSystem
{
	public static class Messages
	{
		#region Constants

		public const string NoExportPath = "Export directory was't selected";

		public const string NothingSelected = "There was no log file selected";

		public const string RootDirDoesntExist = "Directory set as root logs path doesn't exist";

		private const string CollectingLogsDuration = "Collecting logs took: {0}s";

		private const string DirIsEmpty = "Directory is empty: {0}";

		private const string NoLogFile = "Directory doesn't contains log file: {0}";

		private const string ScanningDir = "Scanning directory: {0}";

		private const string UncatchedException = "Something went wrong, exception was throwed with message: {0}";

		public const string ExportSuccess = "Export operation finished successfully";

		private const string CopyingFile = "Copying file '{0}' from {1} to {2}";

		public const string XmlFileLoadSuccess = "File XML successfully loaded";

		private const string ProblemParsingValue = "There was problem with parsing value '{0}' to {1}";

		private const string PropertyNotRecognized = "During loading config, property '{0}' was not recogniozed";

		public const string SettingsLoaded = "Settings loaded successfully";

		public const string SettingsSaved = "Settings saved successfully";

		private const string PropertyAdded = "Property '{0}' added successfully ";

		private const string PropertyUpdated = "Property '{0}' updated successfully ";
		#endregion

		#region Public Methods and Operators

		public static string GetCollectingLogsDuration(double duration)
		{
			return string.Format(CollectingLogsDuration, duration);
		}

		public static string GetDirIsEmpty(string dir)
		{
			return string.Format(DirIsEmpty, dir);
		}

		public static string GetNoLogFile(string dir)
		{
			return string.Format(NoLogFile, dir);
		}

		public static string GetScanningDir(string dir)
		{
			return string.Format(ScanningDir, dir);
		}

		public static string GetUncatchedException(string message)
		{
			return string.Format(UncatchedException, message);
		}

		public static string GetCopyingFile(string name, string from, string to)
		{
			return string.Format(CopyingFile, name, from, to);
		}

		#endregion

		public static string GetProblemParsingValue(string value, string nameOf)
		{
			return string.Format(ProblemParsingValue, value, nameOf);
		}

		public static string GetPropertyNotRecognized(string name)
		{
			return string.Format(PropertyNotRecognized, name);
		}
		public static string GetPropertyAdded(string name)
		{
			return string.Format(PropertyAdded, name);
		}
		public static string GetPropertyUpdated(string name)
		{
			return string.Format(PropertyUpdated, name);
		}
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;

#endregion

namespace LogExplorer.Services.OutputSystem
{
	public static class Messages
	{
		#region Constants

		public const string DeleteAborted = "Delete log operation was aborted";

		public const string DeleteSelectedLogsQuestion = "Are you sure you want to delete selected logs directory?";

		public const string DllNotFound = "Correct dll component was not found";

		public const string ExportSuccess = "Export operation finished successfully";

		public const string NoExportPath = "Export directory was't selected";

		public const string NothingSelected = "There was no log file selected";

		public const string RootDirDoesntExist = "Directory set as root logs path doesn't exist";

		public const string SettingsLoaded = "Settings loaded successfully";

		public const string SettingsSaved = "Settings saved successfully";

		public const string XmlFileLoadSuccess = "XML file was successfully loaded";

		private const string CantCreateDir = "Directory cannot be created: {0}";

		private const string CollectingLogsDuration = "Collecting logs took: {0}s";

		private const string CombineProblem = "There was problm with combining paths '{0}' and '{1}': {2}";

		private const string ConfigNotFound = "Direcotry does not contains config file: {0}";

		private const string CopyingFile = "Copying file '{0}' from {1} to {2}";

		private const string CreateDirSuccess = "Directory created: {0}";

		private const string DeleteOneLogQuestion = "Are you sure you want to delete '{0}' log directory from {1}?";

		private const string DeleteSuccess = "Directory '{0}' was deleted successfuly";

		private const string DeletingCounter = "Deleting {0} of {1} logs";

		private const string DeletingDirerctory =
			"Something went wrong while deleting direrctory, exception was throwed with message: {0}";

		private const string DirIsEmpty = "Directory is empty: {0}";

		private const string DirNotExist = "Directory does not exist: {0}";

		private const string DllFound = "Correct dll component was not found: {0}";

		private const string DllNotExist = "Directory with tester libraries does not exist: {0}";

		private const string DllSearchDuration = "Dll searching took: {0}s";

		private const string FileNotExist = "File does not exist: {0}";

		private const string IncorrectConfigPath = "Config path is incorrect: {0}";

		private const string NoLogFile = "Directory doesn't contains log file: {0}";

		private const string NotallFilesDeleted = "Not all files was deleted: {0}";

		private const string ProblemParsingValue = "There was problem with parsing value '{0}' to {1}";

		private const string ProcessException =
			"Something went wrong while starting process '{0}', exception was throwed with message: {1}";

		private const string ProcessNotStart = "Process '{0}' could not started";

		private const string PropertyAdded = "Property '{0}' added successfully ";

		private const string PropertyNotRecognized = "During loading config, property '{0}' was not recogniozed";

		private const string PropertyUpdated = "Property '{0}' updated successfully ";

		private const string ReadFileException = "The file could not be read: {0}";

		private const string ScanningDir = "Scanning directory: {0}";

		private const string TesterNotFound = "Tester path is incorrect: {0}";

		private const string UncatchedException = "Something went wrong, exception was throwed with message: {0}";

		#endregion

		#region Public Methods and Operators

		public static string GetCantCreateDir(string message)
		{
			return string.Format(CantCreateDir, message);
		}

		public static string GetCollectingLogsDuration(double duration)
		{
			return string.Format(CollectingLogsDuration, duration);
		}

		public static string GetCombineProblem(string p1, string p2, string message)
		{
			return string.Format(CombineProblem, p1, p2, message);
		}

		public static string GetConfigNotFound(string dir)
		{
			return string.Format(ConfigNotFound, dir);
		}

		public static string GetCopyingFile(string name, string from, string to)
		{
			return string.Format(CopyingFile, name, from, to);
		}

		public static string GetCreateDirSuccess(string path)
		{
			return string.Format(CreateDirSuccess, path);
		}

		public static string GetDeletDirException(string message)
		{
			return string.Format(DeletingDirerctory, message);
		}

		public static string GetDeleteOneLogQuestion(string logName, string startTime)
		{
			return string.Format(DeleteOneLogQuestion, logName, startTime);
		}

		public static string GetDeleteSuccess(string path)
		{
			return string.Format(DeleteSuccess, path);
		}

		public static string GetDeletingCounter(int counter, int count)
		{
			return string.Format(DeletingCounter, counter, count);
		}

		public static string GetDirIsEmpty(string dir)
		{
			return string.Format(DirIsEmpty, dir);
		}

		public static string GetDirNotExist(string path)
		{
			return string.Format(DirNotExist, path);
		}

		public static string GetDllFound(string name)
		{
			return string.Format(DllFound, name);
		}

		public static string GetDllNotExist(string dir)
		{
			return string.Format(DllNotExist, dir);
		}

		public static string GetDllSearch(double duration)
		{
			return string.Format(DllSearchDuration, duration);
		}

		public static string GetFileNotExist(string path)
		{
			return string.Format(FileNotExist, path);
		}

		public static string GetIncorrectConfigPath(string path)
		{
			return string.Format(IncorrectConfigPath, path);
		}

		public static string GetNoLogFile(string dir)
		{
			return string.Format(NoLogFile, dir);
		}

		public static string GetNotallFilesDeleted(IEnumerable<string> files)
		{
			return string.Format(NotallFilesDeleted, string.Join(", ", files));
		}

		public static string GetProblemParsingValue(string value, string nameOf)
		{
			return string.Format(ProblemParsingValue, value, nameOf);
		}

		public static string GetProcessException(string process, string message)
		{
			return string.Format(ProcessException, process, message);
		}

		public static string GetProcessNotStart(string process)
		{
			return string.Format(ProcessNotStart, process);
		}

		public static string GetPropertyAdded(string name)
		{
			return string.Format(PropertyAdded, name);
		}

		public static string GetPropertyNotRecognized(string name)
		{
			return string.Format(PropertyNotRecognized, name);
		}

		public static string GetPropertyUpdated(string name)
		{
			return string.Format(PropertyUpdated, name);
		}

		public static string GetReadFileException(string message)
		{
			return string.Format(ReadFileException, message);
		}

		public static string GetScanningDir(string dir)
		{
			return string.Format(ScanningDir, dir);
		}

		public static string GetTesterNotFound(string path)
		{
			return string.Format(TesterNotFound, path);
		}

		public static string GetUncatchedException(string message)
		{
			return string.Format(UncatchedException, message);
		}

		#endregion
	}
}
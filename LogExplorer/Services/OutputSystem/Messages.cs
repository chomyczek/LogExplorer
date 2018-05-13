// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

namespace LogExplorer.Services.OutputSystem
{
	public static class Messages
	{
		#region Constants

		public const string RootDirDoesntExist = "Directory set as root logs path doesn't exist.";

		private const string CollectingLogsDuration = "Collecting logs took: {0}s";

		private const string DirIsEmpty = "Directory is empty: {0}";

		private const string ScanningDir = "Scanning directory: {0}";

		private const string NoLogFile = "Directory doesn't contains log file: {0}";

		private const string UncatchedException = "Something went wrong, unexpected exception: {0}.";

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

		public static string GetScanningDir(string dir)
		{
			return string.Format(ScanningDir, dir);
		}

		public static string GetNoLogFile(string dir)
		{
			return string.Format(NoLogFile, dir);
		}

		#endregion

		public static string GetUncatchedException(string message)
		{
			return string.Format(UncatchedException, message);
		}
	}
}
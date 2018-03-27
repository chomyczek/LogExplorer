// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

#endregion

namespace LogExplorer.Services.Helpers
{
	public class ResultHelper
	{
		#region Constants

		private const string Blocked = "Blocked";

		private const string Exception = "Exception";

		private const string Failed = "Failed";

		private const string Passed = "Passed";

		private const string Unknown = "Unknown";

		private const string Warning = "Warning";

		private const string Workaround = "Workaround";

		#endregion

		#region Static Fields

		private static readonly string[] ExpectedResults = { Failed, Passed, Exception, Workaround, Warning, Blocked };

		private static readonly Brush BlockedBrush = GetSolidBrush("#91C0E8");

		private static readonly Brush ExceptionBrush = GetSolidBrush("#D2BEFF");

		private static readonly Brush FailedBrush = GetSolidBrush("#FFAAA0");

		private static readonly Brush PassedBrush = GetSolidBrush("#B4FFA0");

		private static readonly Brush UnknownBrush = GetSolidBrush("#BD2025");

		private static readonly Brush WarningWorkaroundBrush = GetSolidBrush("#F0FFA0");

		#endregion

		#region Public Methods and Operators

		public static Brush GetColor(string result)
		{
			switch (result)
			{
				case Failed:
					return FailedBrush;
				case Passed:
					return PassedBrush;
				case Exception:
					return ExceptionBrush;
				case Blocked:
					return BlockedBrush;
				case Warning:
				case Workaround:
					return WarningWorkaroundBrush;
				default:
					return UnknownBrush;
			}
		}

		public static string GetResult(IEnumerable<string> fileNames)
		{
			var result = fileNames.Intersect(ExpectedResults).FirstOrDefault();
			
			return result ?? Unknown;
		}

		#endregion

		#region Methods

		private static Brush GetSolidBrush(string rgb)
		{
			return new SolidColorBrush((Color)ColorConverter.ConvertFromString(rgb));
		}

		#endregion
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using LogExplorer.Models;

#endregion

namespace LogExplorer.Services.Helpers
{
	public class ResultHelper
	{
		#region Constants

		private const string All = "All";

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

		private static readonly string[] AllResults =
		{
			All, Passed, Failed, Blocked, Workaround, Warning, Exception, Unknown
		};

		private static readonly Brush BlockedBrush = GetSolidBrush("#91C0E8");

		private static readonly Brush ExceptionBrush = GetSolidBrush("#D2BEFF");

		private static readonly Brush FailedBrush = GetSolidBrush("#FFAAA0");

		private static readonly Brush PassedBrush = GetSolidBrush("#B4FFA0");

		private static readonly Brush UnknownBrush = GetSolidBrush("#BD2025");

		private static readonly Brush WarningWorkaroundBrush = GetSolidBrush("#F0FFA0");

		#endregion

		#region Public Methods and Operators

		public static List<Result> GetAllResults()
		{
			return AllResults.Select(PrepareResult).ToList();
		}

		public static Result GetResult(IEnumerable<string> fileNames)
		{
			var result = fileNames.Intersect(ExpectedResults).FirstOrDefault();

			return PrepareResult(result ?? Unknown);
		}

		#endregion

		#region Methods

		private static Brush GetSolidBrush(string rgb)
		{
			return new SolidColorBrush((Color)ColorConverter.ConvertFromString(rgb));
		}

		private static Result PrepareResult(string value)
		{
			var result = new Result { Value = value, Name = value };
			switch (value)
			{
				case Failed:
					result.Brush = FailedBrush;
					break;
				case Passed:
					result.Brush = PassedBrush;
					break;
				case Exception:
					result.Brush = ExceptionBrush;
					break;
				case Blocked:
					result.Brush = BlockedBrush;
					break;
				case Warning:
					result.Brush = WarningWorkaroundBrush;
					break;
				case Workaround:
					result.Brush = WarningWorkaroundBrush;
					break;
				case Unknown:
					result.Brush = UnknownBrush;
					break;
				default:
					result.Name = All;
					result.Value = string.Empty;
					break;
			}

			return result;
		}

		#endregion
	}
}
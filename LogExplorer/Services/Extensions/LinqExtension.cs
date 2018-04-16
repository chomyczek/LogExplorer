// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace LogExplorer.Services.Extensions
{
	public static class LinqExtension
	{
		#region Public Methods and Operators

		public static bool ContainsString(
			this string target,
			string value,
			StringComparison comparison = StringComparison.OrdinalIgnoreCase)
		{
			return target.IndexOf(value, comparison) >= 0;
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		#endregion
	}
}
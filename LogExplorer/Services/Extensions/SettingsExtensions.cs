// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Reflection;

using LogExplorer.Models;

#endregion

namespace LogExplorer.Services.Extensions
{
	public static class SettingsExtensions
	{
		#region Public Methods and Operators

		public static void CopyValues(this Settings originalSettings, ref Settings copiedSettings)
		{
			var properties = typeof(Settings).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (var propertyInfo in properties)
			{
				propertyInfo.SetValue(copiedSettings, propertyInfo.GetValue(originalSettings));
			}
		}

		#endregion
	}
}
// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace LogExplorer.Services.Converters
{
	public class VisibiityConverter : IValueConverter
	{
		#region Public Methods and Operators

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("ConvertBack method is unneccessary for now.");
		}

		#endregion
	}
}
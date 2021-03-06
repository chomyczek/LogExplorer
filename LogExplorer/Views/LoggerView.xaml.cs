﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LogExplorer.Customs;

using MvvmCross.Wpf.Views;

namespace LogExplorer.Views
{
	/// <summary>
	/// Interaction logic for LoggerView.xaml
	/// </summary>

	[Region(RegionAttribute.Names.Logger)]
	public partial class LoggerView : MvxWpfView
	{
		public LoggerView()
		{
			InitializeComponent();
		}
	}
}

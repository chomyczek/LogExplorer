﻿// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using LogExplorer.Services.OutputSystem;

using Color = System.Drawing.Color;

#endregion

namespace LogExplorer.Views.Controls
{
	/// <summary>
	/// Interaction logic for Icon.xaml
	/// </summary>
	public partial class Icon : UserControl
	{
		#region Static Fields

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
			"Data",
			typeof(Geometry),
			typeof(Icon));

		public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
			"Fill",
			typeof(Brush),
			typeof(Icon));

		public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
			"Size",
			typeof(double),
			typeof(Icon));

		#endregion

		#region Constructors and Destructors

		public Icon()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		#endregion

		#region Public Properties

		public Geometry Data
		{
			get
			{
				return (Geometry)this.GetValue(DataProperty);
			}
			set
			{
				this.SetValue(DataProperty, value);
			}
		}

		public Brush Fill
		{
			get
			{
				return (Brush)this.GetValue(FillProperty);
			}
			set
			{
				this.SetValue(FillProperty, value);
			}
		}

		public double Size
		{
			get
			{
				return (double)this.GetValue(SizeProperty);
			}
			set
			{
				this.SetValue(SizeProperty, value);
			}
		}

		#endregion
	}
}
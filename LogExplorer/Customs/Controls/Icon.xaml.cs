// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace LogExplorer.Customs.Controls
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
			this.Size = 18;
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
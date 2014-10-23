using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Padstone.Xaml.Controls
{
	public class ScrollZoomBar : Control
	{
		static ScrollZoomBar()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollZoomBar), new FrameworkPropertyMetadata(typeof(ScrollZoomBar)));
		}

		private static void ScrollOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as ScrollZoomBar).scrollOfset = (double)e.NewValue;
		}

		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as ScrollZoomBar).zoom = (double)e.NewValue;
		}

		private static object CoerceScrollOffset(DependencyObject d, object baseValue)
		{
			double _baseValue = (double)baseValue;
			object result = _baseValue >= 0 && _baseValue <=1 ? baseValue : 0.0;
			return result;
		}

		private static object CoerceZoom(DependencyObject d, object baseValue)
		{
			double _baseValue = (double)baseValue;
			object result = _baseValue >= 1 ? baseValue : 1.0;
			return result;
		}

		public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => ScrollOffsetProperty),
			typeof(double),
			typeof(ScrollZoomBar),
			new PropertyMetadata(0.0, ScrollOffsetChanged, CoerceScrollOffset));

		public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => ZoomProperty),
			typeof(double),
			typeof(ScrollZoomBar),
			new PropertyMetadata(1.0, ZoomChanged, CoerceZoom));

		private double scrollOfset;
		private double zoom;

		public double ScrollOffset
		{
			get { return this.scrollOfset; }
			set { this.SetValue(ScrollOffsetProperty, value); }
		}

		public double Zoom
		{
			get { return this.zoom; }
			set { this.SetValue(ZoomProperty, value); }
		}
	}
}

using System.Windows;

namespace Padstone.Xaml.Controls
{
	public sealed class VisualDataPoint : DependencyObject
	{
		private double leftOffset;
		private double topOffset;

		private static void LeftOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as VisualDataPoint).leftOffset = (double)e.NewValue;
		}

		private static void TopOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as VisualDataPoint).topOffset = (double)e.NewValue;
		}

		public static readonly DependencyProperty LeftOffsetProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => LeftOffsetProperty),
			typeof(double),
			typeof(VisualDataPoint),
			new PropertyMetadata(0.0, LeftOffsetChanged));

		public static readonly DependencyProperty TopOffsetProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => TopOffsetProperty),
			typeof(double),
			typeof(VisualDataPoint),
			new PropertyMetadata(0.0, TopOffsetChanged));

		public double LeftOffset
		{
			get { return this.leftOffset; }
			set { SetValue(LeftOffsetProperty, value); }
		}

		public double TopOffset
		{
			get { return this.topOffset; }
			set { SetValue(TopOffsetProperty, value); }
		}

		public object Model { get; private set; }

		public Point AsPoint()
		{
			return new Point(this.LeftOffset, this.TopOffset);
		}

		public VisualDataPoint()
		{
		}

		public VisualDataPoint(object model)
		{
			this.Model = model;
		}
	}
}

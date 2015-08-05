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
			if (!e.OldValue.Equals(e.NewValue))
			{
				(d as ScrollZoomBar).zoom = (double)e.NewValue;
			}
		}
		private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!e.OldValue.Equals(e.NewValue))
			{
				(d as ScrollZoomBar).orientation = (Orientation)e.NewValue;
			}
		}

		private static object CoerceScrollOffset(DependencyObject d, object baseValue)
		{
			double _baseValue = (double)baseValue;
			object result = _baseValue >= 0 ? baseValue : 0.0;
			return result;
		}

		private static object CoerceZoom(DependencyObject d, object baseValue)
		{
			double _baseValue = (double)baseValue;
			object result = _baseValue >= 1 ? baseValue : 1.0;
			return result;
		}

		private double NonCoercedOffset { get; set; }

		public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => ScrollOffsetProperty),
			typeof(double),
			typeof(ScrollZoomBar),
			new PropertyMetadata(0.0, ScrollOffsetChanged, CoerceScrollOffset));

		public static readonly DependencyProperty ZoomPercentProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => ZoomPercentProperty),
			typeof(double),
			typeof(ScrollZoomBar),
			new PropertyMetadata(1.0, ZoomChanged, CoerceZoom));

		public Orientation Orientation
		{
			get { return this.orientation; }
			set { SetValue(OrientationProperty, value); }
		}

		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			PropertyName.FromDependencyProperty(() => OrientationProperty),
			typeof(Orientation),
			typeof(ScrollZoomBar),
			new PropertyMetadata(OrientationBox.HorizontalOrientationBox, OrientationChanged));

		private double scrollOfset;
		private double zoom;
		private Orientation orientation;

		private void IncreaseScrollOffset(object value)
		{
			DragCallbackParameter dragData = value as DragCallbackParameter;
			double startingOffset = (double)dragData.Tag;
			double offsetDelta = this.Orientation == Orientation.Horizontal ? dragData.PreviousMousePositionOffset.X : dragData.PreviousMousePositionOffset.Y;
			this.IncreaseScrollOffset(offsetDelta);
		}

		protected void OnResizeGripDrag(object param)
		{
		}

		protected override Size MeasureOverride(Size constraint)
		{
			double desiredWidth = this.Orientation == Orientation.Horizontal ? constraint.Width : Math.Min(constraint.Width, 15.0);
			double desiredHeight = this.Orientation == Orientation.Vertical ? constraint.Height : Math.Min(constraint.Height, 15.0);
			return new Size(desiredWidth, desiredHeight);
		}

		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			return base.ArrangeOverride(arrangeBounds);
		}

		/// <summary>
		/// Increases the <see cref="ScrollOffset"/> property by the specified amount. A negative value will decrease the <see cref="ScrollOffset"/>.
		/// <param name="value">The amount, in pixels, by which the <see cref="ScrollOffset"/> will increase or decrease.</param>
		/// </summary>
		public void IncreaseScrollOffset(double value)
		{
			this.ScrollOffset += value;
		}

		/// <summary>
		/// 
		/// </summary>
		public double ScrollOffset
		{
			get { return this.scrollOfset; }
			set { this.SetValue(ScrollOffsetProperty, value); }
		}

		public double ZoomPercent
		{
			get { return this.zoom; }
			set { this.SetValue(ZoomPercentProperty, value); }
		}

		public ICommand IncreaseScrollOffsetCommand { get; private set; }

		public ICommand OnInnerResizeGripDragCommand { get; private set; }

		public ICommand OnOuterResizeGripDragCommand { get; private set; }

		public ScrollZoomBar()
		{
			this.IncreaseScrollOffsetCommand = new DelegateCommand(this.IncreaseScrollOffset);
		}
	}
}

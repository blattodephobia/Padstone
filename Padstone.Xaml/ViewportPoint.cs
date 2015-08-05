using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Padstone.Xaml
{
	/// <summary>
	/// Internal reference type substitute for System.Windows.Point for use with DragService. Prevents unnecessary boxing/unboxing
	/// </summary>
	internal class ViewportPoint
	{
		public double X { get; set; }
		public double Y { get; set; }

		public static explicit operator Point(ViewportPoint viewportPoint)
		{
			return new Point(viewportPoint.X, viewportPoint.Y);
		}

		public ViewportPoint()
		{
		}

		public ViewportPoint(Point point) :
			this(point.X, point.Y)
		{
		}

		public ViewportPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}

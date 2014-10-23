using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Padstone.Xaml.Controls
{
	public abstract class AxisBase : FrameworkElement, IValueToViewportLocationConverter
	{
		public VisualDataPoint ValueToViewportLocation(object value)
		{
			throw new NotImplementedException();
		}

		public object ViewportPointToValue(Point location)
		{
			throw new NotImplementedException();
		}
	}
}
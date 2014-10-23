using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Padstone.Xaml.Controls
{
	public interface IValueToViewportLocationConverter
	{
		VisualDataPoint ValueToViewportLocation(object value);

		object ViewportPointToValue(Point location);
	}
}
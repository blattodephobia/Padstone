using System;
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

namespace Padstone.Xaml.Controls
{
	public abstract class DataVisualizationControl : Control
	{
		static DataVisualizationControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(DataVisualizationControl), new FrameworkPropertyMetadata(typeof(DataVisualizationControl)));
		}
	}
}

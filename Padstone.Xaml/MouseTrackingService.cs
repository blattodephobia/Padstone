using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Padstone.Xaml
{
	internal sealed class MouseTrackingService : DependencyObject
	{
		
		public static void Register(UIElement element)
		{
			element.MouseLeftButtonDown += element_MouseLeftButtonDown;
			element.MouseMove += element_MouseMove;
			element.MouseLeftButtonUp += element_MouseLeftButtonUp;
			InputManager.Current.PrimaryMouseDevice.Capture(element);
		}

		static void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			
		}

		static void element_MouseMove(object sender, MouseEventArgs e)
		{
			throw new NotImplementedException();
		}

		static void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			UIElement target = sender as UIElement;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Padstone.Xaml.Markup
{
	public class InputDataBinding : MarkupExtension
	{
		private UIElement Target { get; set; }
		private void RegisterToHandlers(UIElement target)
		{
			if (this.Device == InputDevice.Mouse)
			{
				MouseTrackingService.Register(target);
			}
		}

		public InputGesture CaptureDeviceGesture { get; set; }

		public InputGesture ReleaseDeviceGesture { get; set; }

		public InputDevice Device { get; set; }

		public InputDataBinding()
		{
			
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			IProvideValueTarget targetInfo = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			UIElement target = targetInfo.TargetObject as UIElement;
			DependencyProperty targetProprety = targetInfo.TargetProperty as DependencyProperty;
			if (targetProprety != null)
			{
				return target.GetValue(targetProprety);
			}

			PropertyInfo alternateProperty = targetInfo.TargetProperty as PropertyInfo;
			if (alternateProperty != null)
			{
				return alternateProperty.GetGetMethod().Invoke(target, null);
			}
			RegisterToHandlers(target);
			throw new InvalidOperationException();
		}
	}
}

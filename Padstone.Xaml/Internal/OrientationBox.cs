using System.Windows.Controls;

namespace Padstone.Xaml
{
	internal static class OrientationBox
	{
		public static readonly object HorizontalOrientationBox = Orientation.Horizontal;

		public static readonly object VerticalOrientationBox = Orientation.Vertical;

		public static object Box(Orientation value)
		{
			return value == Orientation.Horizontal ? HorizontalOrientationBox : VerticalOrientationBox;
		}
	}
}

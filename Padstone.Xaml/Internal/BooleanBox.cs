using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Xaml
{
	internal static class BooleanBox
	{
		public static readonly object TrueBox = true;

		public static readonly object FalseBox = false;

		public static object BoxValue(bool value)
		{
			return value ? TrueBox : FalseBox;
		}
	}
}

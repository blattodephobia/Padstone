using System.Windows;

namespace Padstone.Xaml.Controls
{
	public class DragCallbackParameter
	{
		public Point DragOrigin { get; internal set; }

		/// <summary>
		/// Gets the coordinates of the pixel that the input device is currently pointing at.
		/// </summary>
		public Point CurrentHotSpotPosition { get; internal set; }

		public Vector PreviousMousePositionOffset { get; internal set; }

		public Vector DragOriginOffset
		{
			get
			{
				return this.CurrentHotSpotPosition - this.DragOrigin;
			}
		}

		public object StartData { get; set; }

		public object Tag { get; set; }

		public DragCallbackParameter()
		{
		}

		public DragCallbackParameter(Point dragOrigin, Point currentPostion, object tag = null) :
			this(Point.Subtract(currentPostion, dragOrigin), tag)
		{
		}

		public DragCallbackParameter(Vector offset, object tag = null)
		{
			this.PreviousMousePositionOffset = offset;
			this.Tag = tag;
		}
	}
}
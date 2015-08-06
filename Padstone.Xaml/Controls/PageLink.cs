using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Xaml.Controls
{
    /// <summary>
    /// Represents a navigational link to a page in a <see cref="DataPager"/>
    /// </summary>
    public class PageLink
    {
        /// <summary>
        /// Gets the zero-based index of an item in a collection of pages.
        /// </summary>
        public int PageIndex { get; private set; }


        /// <summary>
        /// Gets the one-based index of an item in a collection of pages.
        /// </summary>
        public int PageNumber
        {
            get
            {
                return this.PageIndex + 1;
            }
        }

        /// <summary>
        /// Gets a string used to represent the <see cref="PageLink"/> in the presentation layer.
        /// This property's value corresponds to the <see cref="PageNumber"/> property, unless the latter is zero or lower.
        /// In that case, the <see cref="DisplayString"/> returns an ellipsis.
        /// </summary>
        public string DisplayString
        {
            get
            {
                return this.PageIndex > -1 ? this.PageNumber.ToString() : "...";
            }
        }

        /// <summary>
        /// Creates a new isntance of the <see cref="PageLink"/> class.
        /// </summary>
        /// <param name="index">The zero-based index of the page in its collection.</param>
        public PageLink(int index)
        {
            this.PageIndex = index;
        }

        public override string ToString()
        {
            return this.DisplayString;
        }
    }
}

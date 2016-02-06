using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data.Internal
{
    internal class Helpers
    {
        public static Func<T, T, int> GetComparison<T>(IComparer<T> comparer)
        {
            return comparer.Compare;
        }

        public static Func<T, T, int> GetComparison<T>(Comparison<T> comparison)
        {
            return comparison.Invoke;
        }

        public static Func<T, T, int> GetComparison<T>()
        {
            if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
            {
                return (T x, T y) => ((IComparable<T>)x).CompareTo(y);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}

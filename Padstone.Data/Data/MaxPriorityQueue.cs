using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public class MaxPriorityQueue<T> : HeapBase<T>
    {
        public MaxPriorityQueue() :
            base()
        {
        }

        public MaxPriorityQueue(IComparer<T> comparer) :
            base(comparer)
        {
        }

        public MaxPriorityQueue(Func<T, T, int> comparison) : 
            base(comparison)
        {
        }

        protected override int PriorityCompare(T first, T second)
        {
            return -this.Comparer.Invoke(first, second);
        }
    }
}

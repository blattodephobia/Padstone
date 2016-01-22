using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public class MinPriorityQueue<T> : HeapBase<T>
    {
        public MinPriorityQueue() :
            base()
        {
        }

        public MinPriorityQueue(IComparer<T> comparer) :
            base(comparer)
        {
        }

        public MinPriorityQueue(Func<T, T, int> comparison) : 
            base(comparison)
        {
        }

        protected override int PriorityCompare(T first, T second)
        {
            return this.Comparer.Invoke(first, second);
        }
    }
}

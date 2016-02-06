using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data.Internal
{
    [DebuggerDisplay("'{Key}'({Priority})")]
    internal class TreapNode<T>
    {
        private static readonly Random Rand = new Random();

        internal static Func<int> PriorityProvider { get; set; }

        public TreapNode<T> Parent { get; set; }

        public TreapNode<T> Left { get; set; }

        public TreapNode<T> Right { get; set; }

        public T Key { get; private set; }

        public int Priority { get; private set; }

        public TreapNode(T key) :
            this(key, PriorityProvider?.Invoke() ?? Rand.Next(int.MinValue, int.MaxValue))
        {
        }

        public TreapNode(T key, int priority)
        {
            this.Key = key;
            this.Priority = priority;
        }
    }
}

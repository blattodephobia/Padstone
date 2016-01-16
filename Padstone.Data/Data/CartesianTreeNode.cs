using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    internal class CartesianTreeNode<T>
    {
        public CartesianTreeNode<T> Parent { get; set; }
        public CartesianTreeNode<T> Left { get; set; }
        public CartesianTreeNode<T> Right { get; set; }
        public T Value { get; set; }
        public int Index { get; set; }
    }
}

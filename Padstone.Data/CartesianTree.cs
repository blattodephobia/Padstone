using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public class CartesianTree<T>
    {
        private CartesianTreeNode<T> root;

        private int IComparableResult(T x, T y)
        {
            IComparable<T> _x = x as IComparable<T>;
            return _x.CompareTo(y);
        }

        private Comparison<T> Compare;
        private int itemsCount = 0;

        public void AddRange(IEnumerable<T> collection)
        {
            CartesianTreeNode<T> current = null;
            int currentIndex = this.itemsCount - 1;
            foreach (T item in collection)
            {
                currentIndex++;
                if (this.root == null)
                {
                    this.root = new CartesianTreeNode<T>() { Index = 0, Value = item };
                    current = this.root;
                    continue;
                }

                if (this.Compare(root.Value, item) < 0)
                {
                    current.Right = new CartesianTreeNode<T>() { Index = currentIndex, Value = item, Parent = current };
                    current = current.Right;
                }
                else
                {
                    while (current.Parent != null && this.Compare(current.Parent.Value, item) >= 0)
                    {
                        current = current.Parent;
                    }
                    CartesianTreeNode<T> oldParent = current.Parent;
                    current.Parent = new CartesianTreeNode<T>() { Index = currentIndex, Value = item, Left = current };
                    current = current.Parent;
                    if (oldParent != null)
                    {
                        oldParent.Right = current;
                        current.Parent = oldParent;
                    }
                }

                if (this.Compare(current.Value, this.root.Value) <= 0)
                {
                    this.root = current;
                }
            }
        }

        public T Min(int rangeInclusiveStart, int rangeExclusiveEnd)
        {
            CartesianTreeNode<T> current = this.root;
            while (current.Index < rangeInclusiveStart || current.Index > rangeExclusiveEnd)
            {
                if (current.Index < rangeInclusiveStart)
                {
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }
            }

            return current.Value;
        }

        public CartesianTree() :
            this(Enumerable.Empty<T>())
        {
        }

        private bool AllTrue(Type type, object criteria)
        {
            return true;
        }

        public CartesianTree(IEnumerable<T> collection)
        {
            Type icomparableType = typeof(IComparable<T>);
            if (typeof(T).FindInterfaces((type, criteria) => type == icomparableType, null).Any())
            {
                this.Compare = this.IComparableResult;
            }

            this.AddRange(collection);
        }

        public CartesianTree(IEnumerable<T> collection, IComparer<T> comparer)
        {
            this.Compare = comparer.Compare;
            this.AddRange(collection);
        }

        public CartesianTree(IEnumerable<T> collection, Comparison<T> comparison)
        {
            this.Compare = comparison;
            this.AddRange(collection);
        }
    }
}

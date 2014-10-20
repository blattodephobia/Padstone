using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public class CartesianTree<T> : IList<T>
    {
        private CartesianTreeNode<T> root;
        private CartesianTreeNode<T> lastAddedNode;

        private static int IComparableResult(T x, T y)
        {
            IComparable<T> _x = x as IComparable<T>;
            return _x.CompareTo(y);
        }

        private Comparison<T> GetInvertedComparison(Comparison<T> comparison)
        {
            Comparison<T> result = delegate (T x, T y)
            {
                return -comparison.Invoke(x, y);
            };

            return result;
        }

        private Comparison<T> Compare;

        private List<T> listInternal = new List<T>();

        private void Initialize(IEnumerable<T> collection, Comparison<T> comparison, bool selectMaxValues)
        {
            this.Compare = selectMaxValues ? this.GetInvertedComparison(comparison) : comparison;
            this.IsMaxQueryTree = selectMaxValues;
            this.AddRange(collection);
        }

        private void AddInternal(T item)
        {
            int currentIndex = this.Count;
            if (this.root == null)
            {
                this.root = new CartesianTreeNode<T>() { Index = 0, Value = item };
                this.lastAddedNode = this.root;
                this.listInternal.Add(item);
            }
            else
            {
                if (this.Compare(root.Value, item) < 0)
                {
                    this.lastAddedNode.Right = new CartesianTreeNode<T>() { Index = currentIndex, Value = item, Parent = this.lastAddedNode };
                    this.lastAddedNode = this.lastAddedNode.Right;
                }
                else
                {
                    while (this.lastAddedNode.Parent != null && this.Compare(this.lastAddedNode.Parent.Value, item) >= 0)
                    {
                        this.lastAddedNode = this.lastAddedNode.Parent;
                    }
                    CartesianTreeNode<T> oldParent = this.lastAddedNode.Parent;
                    this.lastAddedNode.Parent = new CartesianTreeNode<T>() { Index = currentIndex, Value = item, Left = this.lastAddedNode };
                    this.lastAddedNode = this.lastAddedNode.Parent;
                    if (oldParent != null)
                    {
                        oldParent.Right = this.lastAddedNode;
                        this.lastAddedNode.Parent = oldParent;
                    }
                }

                if (this.Compare(this.lastAddedNode.Value, this.root.Value) <= 0)
                {
                    this.root = this.lastAddedNode;
                }

                this.listInternal.Add(item);
            }
        }

        public T GetRangeExtremum(int rangeInclusiveStart, int rangeExclusiveEnd)
        {
            if (rangeInclusiveStart < 0 || rangeExclusiveEnd > this.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            CartesianTreeNode<T> current = this.root;
            while (current.Index < rangeInclusiveStart || current.Index >= rangeExclusiveEnd)
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

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                this.AddInternal(item);
            }
        }

        public void Add(T item)
        {
            this.AddInternal(item);
        }

        public void Clear()
        {
            this.listInternal.Clear();
            this.root = null;
        }

        public bool Contains(T item)
        {
            return this.listInternal.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.listInternal.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return this.listInternal.IndexOf(item);
        }

        public int Count
        {
            get
            {
                return this.listInternal.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.listInternal[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsMaxQueryTree { get; private set; }

        public CartesianTree() :
            this(Enumerable.Empty<T>(), false)
        {
        }

        public CartesianTree(bool selectMaxValues) :
            this(Enumerable.Empty<T>(), selectMaxValues)
        {
        }

        public CartesianTree(IEnumerable<T> collection, bool selectMaxValues = false)
        {
            Type icomparableType = typeof(IComparable<T>);
            if (typeof(T).FindInterfaces((type, criteria) => type == icomparableType, null).Any())
            {
                Comparison<T> comparison = IComparableResult;
                this.Initialize(collection, comparison, selectMaxValues);
            }
            else
            {
                throw new NotSupportedException(string.Format("Type {0} doesn't implement IComparable<{0}>", typeof(T).FullName));
            }
        }

        public CartesianTree(IEnumerable<T> collection, IComparer<T> comparer, bool selectMaxValues = false)
        {
            Comparison<T> comparison = comparer.Compare;
            this.Initialize(collection, comparison, selectMaxValues);
        }

        public CartesianTree(IEnumerable<T> collection, Comparison<T> comparison, bool selectMaxValues = false)
        {
            this.Initialize(collection, comparison, selectMaxValues);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.listInternal.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

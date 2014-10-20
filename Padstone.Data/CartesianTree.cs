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

        public void AddRange(IEnumerable<T> collection)
        {
            CartesianTreeNode<T> current = null;
            int currentIndex = this.Count - 1;
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
                listInternal.Add(item);
            }
        }

        public T RangeExtremum(int rangeInclusiveStart, int rangeExclusiveEnd)
        {
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

        #region IList<T> methods

        public int IndexOf(T item)
        {
            return this.listInternal.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}

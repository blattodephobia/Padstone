using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public abstract class HeapBase<T>
    {
        private static int GetLeftChildIndex(int currentElementIndex)
        {
            return currentElementIndex * 2 + 1;
        }

        private static int GetRightChildIndex(int currentElementIndex)
        {
            return currentElementIndex * 2 + 2;
        }

        private static int GetParentIndex(int currentElementIndex)
        {
            return currentElementIndex / 2;
        }

        private List<T> internalList = new List<T>();

        protected HeapBase()
        {
            if (!typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"The default constructor of this type expects that the generic type argument T implements {typeof(IComparable<>)}.");
            }
            this.Comparer = (x, y) => ((IComparable<T>)x).CompareTo(y);
        }

        protected HeapBase(IComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            this.Comparer = (x, y) => comparer.Compare(x, y);
        }

        protected HeapBase(Func<T, T, int> comparison)
        {
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));

            this.Comparer = comparison;
        }

        public void Add(T item)
        {
            this.internalList.Add(item);
            this.PercolateUp(this.internalList.Count - 1);
        }

        public T Remove()
        {
            T result = this.internalList.First();

            this.Swap(0, this.internalList.Count - 1);
            this.internalList.RemoveAt(this.internalList.Count - 1);
            this.PercolateDown(0);

            return result;
        }

        public T Peek() => this.internalList.First();

        public int Count
        {
            get
            {
                return this.internalList.Count;
            }
        }

        /// <summary>
        /// Gets an integer indicating the order in which the parameters should appear in an ordered sequence.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>-1 if <paramref name="first"/> should come before <paramref name="second"/>, 0 if they have equal values, or 1 if <paramref name="first"/> should come after <paramref name="second"/>.</returns>
        protected abstract int PriorityCompare(T first, T second);

        protected Func<T, T, int> Comparer { get; private set; }

        protected void PercolateUp(int index)
        {
            int parentIndex = GetParentIndex(index);
            if (this.PriorityCompare(this.internalList[index], this.internalList[parentIndex]) < 0)
            {
                this.Swap(index, parentIndex);
                this.PercolateUp(parentIndex);
            }
        }

        protected void PercolateDown(int index)
        {
            int bottomElementIndex = index;
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);

            bottomElementIndex = this.IsInRange(leftChildIndex) && this.PriorityCompare(this.internalList[bottomElementIndex], this.internalList[leftChildIndex]) >= 0
                ? leftChildIndex
                : bottomElementIndex;
            bottomElementIndex = this.IsInRange(rightChildIndex) && this.PriorityCompare(this.internalList[bottomElementIndex], this.internalList[rightChildIndex]) >= 0
                ? rightChildIndex
                : bottomElementIndex;

            if (bottomElementIndex != index)
            {
                this.Swap(bottomElementIndex, index);
                this.PercolateDown(bottomElementIndex);
            }
        }

        private void Swap(int elementIndex1, int elementIndex2)
        {
            if (elementIndex1 != elementIndex2)
            {
                T tmp = this.internalList[elementIndex1];
                this.internalList[elementIndex1] = this.internalList[elementIndex2];
                this.internalList[elementIndex2] = tmp;
            }
        }

        private bool IsInRange(int index)
        {
            return 0 <= index && index < this.internalList.Count;
        }
    }
}

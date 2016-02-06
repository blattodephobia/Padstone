using Padstone.Data.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public class Treap<T>
    {
        private readonly Func<int> randomPriorityProvider;
        private readonly Func<T, T, int> comparer;
        private TreapNode<T> root;

        public Treap(Func<T, T, int> comparison)
        {
            this.comparer = comparison;
            Random rand = new Random();
            this.randomPriorityProvider = () => rand.Next(int.MinValue, int.MaxValue);
        }

        public Treap() :
            this(Helpers.GetComparison<T>())
        {
        }

        public Treap(IComparer<T> comparer) :
            this(Helpers.GetComparison(comparer))
        {
        }

        internal Treap(Func<int> randomPriorityProvider)
        {
            if (randomPriorityProvider == null) throw new ArgumentNullException(nameof(randomPriorityProvider));

            this.comparer = Helpers.GetComparison<T>();
            this.randomPriorityProvider = randomPriorityProvider;
        }

        public void Add(T item)
        {
            TreapNode<T> node = this.CreateNode(item);
            TreapNode<T> parent = this.GetParent(item);
            if (parent != null)
            {
                if (this.comparer.Invoke(parent.Key, item) < 0)
                {
                    parent.Right = node;
                }
                else
                {
                    parent.Left = node;
                }

                node.Parent = parent;
            }
            else
            {
                this.root = node;
            }

            this.Balance(node);
        }

        public bool Contains(T item)
        {
            TreapNode<T> parent = this.GetParent(item);
            return (parent?.Left  != null && this.comparer.Invoke(parent.Left.Key,  item) == 0) ||
                   (parent?.Right != null && this.comparer.Invoke(parent.Right.Key, item) == 0) ||
                   (parent        != null && this.comparer.Invoke(parent.Key,       item) == 0);
        }

        private TreapNode<T> CreateNode(T key)
        {
            return new TreapNode<T>(key, this.randomPriorityProvider.Invoke());
        }

        private void Balance(TreapNode<T> insertedNode)
        {
            while ((insertedNode?.Parent?.Priority ?? int.MaxValue) < (insertedNode?.Priority ?? int.MinValue))
            {
                insertedNode = object.ReferenceEquals(insertedNode.Parent.Left, insertedNode)
                    ? this.RotateRight(insertedNode.Parent)
                    : this.RotateLeft(insertedNode.Parent);

                if (insertedNode != null && insertedNode.Parent == null) this.root = insertedNode;
            }
        }

        private TreapNode<T> RotateRight(TreapNode<T> subTree)
        {
            var parent = subTree?.Parent;

            if (subTree?.Left != null)
            {
                TreapNode<T> left = subTree.Left;
                TreapNode<T> leftRightDescendant = left.Right;

                subTree.Left = leftRightDescendant;
                left.Right = subTree;
                subTree = left;
            }

            this.AttachToParent(subTree, parent);

            return subTree;
        }

        private TreapNode<T> RotateLeft(TreapNode<T> subTree)
        {
            var parent = subTree?.Parent;

            if (subTree?.Right != null)
            {
                TreapNode<T> right = subTree.Right;
                TreapNode<T> rightLeftDescendant = right.Left;

                subTree.Right = rightLeftDescendant;
                right.Left = subTree;
                subTree.Parent = right;
                subTree = right;
            }

            this.AttachToParent(subTree, parent);

            return subTree;
        }

        private void AttachToParent(TreapNode<T> subTree, TreapNode<T> parent)
        {
            if (parent != null)
            {
                if (this.comparer.Invoke(subTree.Key, parent.Key) < 0)
                {
                    parent.Left = subTree;
                }
                else
                {
                    parent.Right = subTree;
                }
            }
            subTree.Parent = parent;
        }

        private TreapNode<T> GetParent(T item)
        {
            TreapNode<T> parent = null;
            TreapNode<T> current = this.root;
            while (current != null)
            {
                parent = current;
                if (parent != null && this.comparer.Invoke(parent.Key, item) == 0)
                {
                    return parent;
                }

                current = this.comparer.Invoke(item, current.Key) < 0
                ? current.Left
                : current.Right;
            }

            return parent;
        }
    }
}

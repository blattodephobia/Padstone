using Microsoft.VisualStudio.TestTools.UnitTesting;
using Padstone.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Tests.Data
{
    [TestClass]
    public class MinPriorityQueueTests
    {
        class CustomComparable : IComparable<CustomComparable>
        {
            public int Value { get; private set; }

            public CustomComparable(int value)
            {
                this.Value = value;
            }

            public int CompareTo(CustomComparable other)
            {
                return this.Value.CompareTo(other.Value);
            }

            public int Compare(CustomComparable x, CustomComparable y)
            {
                return x.CompareTo(y);
            }
        }

        class CustomPoco
        {
            public int Value { get; private set; }

            public CustomPoco(int value)
            {
                this.Value = value;
            }
        }

        class CustomComparer : IComparer<CustomPoco>
        {
            public int Compare(CustomPoco x, CustomPoco y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }

        [TestClass]
        public class AddTests
        {
            [TestMethod]
            public void ShouldAddTwoItemsInOrder()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();
                heap.Add(3);
                heap.Add(5);

                Assert.AreEqual(3, heap.Peek());
            }

            [TestMethod]
            public void ShouldAddTwoItemsInReverseOrder()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();
                heap.Add(5);
                heap.Add(3);

                Assert.AreEqual(3, heap.Peek());
            }

            [TestMethod]
            public void ShouldAddMultipleItemsRandomly()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();
                heap.Add(5);
                heap.Add(3);
                heap.Add(6);
                heap.Add(2);
                heap.Add(1);

                Assert.AreEqual(1, heap.Peek());
            }

            [TestMethod]
            public void ShouldAddMultipleItemsWithCustomIComparer()
            {
                MinPriorityQueue<CustomPoco> heap = new MinPriorityQueue<CustomPoco>(new CustomComparer());
                heap.Add(new CustomPoco(5));
                heap.Add(new CustomPoco(3));
                heap.Add(new CustomPoco(6));
                heap.Add(new CustomPoco(2));
                heap.Add(new CustomPoco(1));

                Assert.AreEqual(1, heap.Peek().Value);
            }

            [TestMethod]
            public void ShouldAddMultipleItemsWithCustomComparisonDelegate()
            {
                MinPriorityQueue<CustomPoco> heap = new MinPriorityQueue<CustomPoco>((x, y) => x.Value.CompareTo(y.Value));
                heap.Add(new CustomPoco(5));
                heap.Add(new CustomPoco(3));
                heap.Add(new CustomPoco(6));
                heap.Add(new CustomPoco(2));
                heap.Add(new CustomPoco(1));

                Assert.AreEqual(1, heap.Peek().Value);
            }
        }

        [TestClass]
        public class RemoveTests
        {
            [TestMethod]
            public void ShouldRemoveCorrectly1()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();
                heap.Add(3);
                heap.Add(5);

                Assert.AreEqual(3, heap.Remove());
                Assert.AreEqual(5, heap.Peek());
            }

            [TestMethod]
            public void ShouldRemoveCorrectly2()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();
                heap.Add(5);
                heap.Add(3);
                heap.Add(6);
                heap.Add(2);
                heap.Add(1);

                Assert.AreEqual(1, heap.Remove());
                Assert.AreEqual(2, heap.Remove());
                Assert.AreEqual(3, heap.Remove());
                Assert.AreEqual(5, heap.Remove());
                Assert.AreEqual(6, heap.Remove());
            }

            [TestMethod]
            public void ShouldRemoveCorrectlyWithCustomComparer()
            {
                MinPriorityQueue<CustomPoco> heap = new MinPriorityQueue<CustomPoco>(new CustomComparer());
                heap.Add(new CustomPoco(5));
                heap.Add(new CustomPoco(3));
                heap.Add(new CustomPoco(6));
                heap.Add(new CustomPoco(2));
                heap.Add(new CustomPoco(1));

                Assert.AreEqual(1, heap.Remove().Value);
                Assert.AreEqual(2, heap.Remove().Value);
                Assert.AreEqual(3, heap.Remove().Value);
                Assert.AreEqual(5, heap.Remove().Value);
                Assert.AreEqual(6, heap.Remove().Value);
            }

            [TestMethod]
            public void ShouldRemoveCorrectlyWithCustomComparisonDelegate()
            {
                MinPriorityQueue<CustomPoco> heap = new MinPriorityQueue<CustomPoco>((x, y) => x.Value.CompareTo(y.Value));
                heap.Add(new CustomPoco(5));
                heap.Add(new CustomPoco(3));
                heap.Add(new CustomPoco(6));
                heap.Add(new CustomPoco(2));
                heap.Add(new CustomPoco(1));

                Assert.AreEqual(1, heap.Remove().Value);
                Assert.AreEqual(2, heap.Remove().Value);
                Assert.AreEqual(3, heap.Remove().Value);
                Assert.AreEqual(5, heap.Remove().Value);
                Assert.AreEqual(6, heap.Remove().Value);
            }
        }

        [TestClass]
        public class CountTests
        {
            [TestMethod]
            public void SetsCountCorrectly1()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();
                Assert.AreEqual(0, heap.Count);

                heap.Add(5);
                Assert.AreEqual(1, heap.Count);
            }

            [TestMethod]
            public void SetsCountCorrectly2()
            {
                MinPriorityQueue<int> heap = new MinPriorityQueue<int>();

                heap.Add(5);
                Assert.AreEqual(1, heap.Count);

                heap.Remove();
                Assert.AreEqual(0, heap.Count);
            }
        }

        [TestClass]
        public class CtorTests
        {
            [TestMethod]
            public void ShouldAcceptCustomIComparables()
            {
                MinPriorityQueue<CustomComparable> heap = new MinPriorityQueue<CustomComparable>();
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldThrowOnNonIComparables()
            {
                MinPriorityQueue<object> heap = new MinPriorityQueue<object>();
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ShouldThrowOnNullIComparer()
            {
                MinPriorityQueue<CustomPoco> heap = new MinPriorityQueue<CustomPoco>(comparer: null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ShouldThrowOnNullComparison()
            {
                MinPriorityQueue<CustomPoco> heap = new MinPriorityQueue<CustomPoco>(comparison: null);
            }
        }
    }
}

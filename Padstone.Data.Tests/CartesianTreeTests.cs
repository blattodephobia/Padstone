using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Padstone.Data.Tests
{
    [TestClass]
    public class CartesianTreeTests
    {
        [TestMethod]
        public void Test_Min_NonRepeatingSequence()
        {
            CartesianTree<int> testTree = new CartesianTree<int>();
            int[] sample = new int[] { 9, 3, 7, 1, 8, 12, 10, 20, 15, 18, 5 };
            testTree.AddRange(sample);
            Assert.AreEqual(1, testTree.GetRangeExtremum(0, sample.Length));
            Assert.AreEqual(8, testTree.GetRangeExtremum(4, 7));
            Assert.AreEqual(10, testTree.GetRangeExtremum(6, 8));
            Assert.AreEqual(7, testTree.GetRangeExtremum(2, 3));
        }

        [TestMethod]
        public void Test_Min_RepeatingSequence()
        {
            CartesianTree<int> testTree = new CartesianTree<int>() { 9, 9, 7, 7, 8, 12, 5, 20, 8, 18, 5 };
            Assert.AreEqual(9, testTree.GetRangeExtremum(0, 1));
            Assert.AreEqual(8, testTree.GetRangeExtremum(4, 5));
            Assert.AreEqual(5, testTree.GetRangeExtremum(4, testTree.Count));
        }

        [TestMethod]
        public void Test_Min_Single()
        {
            CartesianTree<int> testTree = new CartesianTree<int>();
            int[] sample = new int[] { 9 };
            testTree.AddRange(sample);
            Assert.AreEqual(9, testTree.GetRangeExtremum(0, 1));
        }
        [TestMethod]
        public void Test_Max_NonRepeatingSequence()
        {
            CartesianTree<int> testTree = new CartesianTree<int>(true);
            int[] sample = new int[] { 9, 3, 7, 1, 8, 12, 10, 20, 15, 18, 5 };
            testTree.AddRange(sample);
            Assert.AreEqual(20, testTree.GetRangeExtremum(0, sample.Length));
            Assert.AreEqual(12, testTree.GetRangeExtremum(4, 7));
            Assert.AreEqual(18, testTree.GetRangeExtremum(9, 11));
        }

        [TestMethod]
        public void Test_Max_RepeatingSequence()
        {
            CartesianTree<int> testTree = new CartesianTree<int>(true);
            int[] sample = new int[] { 9, 9, 7, 7, 8, 12, 5, 20, 8, 18, 5 };
            testTree.AddRange(sample);
            Assert.AreEqual(9, testTree.GetRangeExtremum(0, 2));
            Assert.AreEqual(8, testTree.GetRangeExtremum(4, 5));
            Assert.AreEqual(20, testTree.GetRangeExtremum(4, sample.Length));
            Assert.AreEqual(12, testTree.GetRangeExtremum(0, 7));
        }

        [TestMethod]
        public void Test_Max_Single()
        {
            CartesianTree<int> testTree = new CartesianTree<int>(true);
            int[] sample = new int[] { 9 };
            testTree.AddRange(sample);
            Assert.AreEqual(9, testTree.GetRangeExtremum(0, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_UnsupportedType()
        {            
            CartesianTree<object> testTree = new CartesianTree<object>();
            testTree.AddRange(new object[] { 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_OutOfRange()
        {
            CartesianTree<int> testTree = new CartesianTree<int>() { 3, 4, 1 };
            Assert.AreEqual(1, testTree.GetRangeExtremum(0, 4));
        }
    }
}

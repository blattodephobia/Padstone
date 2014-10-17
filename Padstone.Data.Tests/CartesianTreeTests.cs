using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Padstone.Data.Tests
{
    [TestClass]
    public class CartesianTreeTests
    {
        [TestMethod]
        public void Test_NonRepeatingSequence()
        {
            CartesianTree<int> testTree = new CartesianTree<int>();
            int[] sample = new int[] { 9, 3, 7, 1, 8, 12, 10, 20, 15, 18, 5 };
            testTree.AddRange(sample);
            Assert.AreEqual(1, testTree.Min(0, sample.Length));
            Assert.AreEqual(8, testTree.Min(4, 7));
            Assert.AreEqual(10, testTree.Min(6, 8));
        }

        [TestMethod]
        public void Test_RepeatingSequence()
        {
            CartesianTree<int> testTree = new CartesianTree<int>();
            int[] sample = new int[] { 9, 9, 7, 7, 8, 12, 5, 20, 8, 18, 5 };
            testTree.AddRange(sample);
            Assert.AreEqual(9, testTree.Min(0, 1));
            Assert.AreEqual(8, testTree.Min(4, 5));
            Assert.AreEqual(5, testTree.Min(4, sample.Length));
        }

        [TestMethod]
        public void Test_Single()
        {
            CartesianTree<int> testTree = new CartesianTree<int>();
            int[] sample = new int[] { 9 };
            testTree.AddRange(sample);
            Assert.AreEqual(9, testTree.Min(0, 1));
        }
    }
}

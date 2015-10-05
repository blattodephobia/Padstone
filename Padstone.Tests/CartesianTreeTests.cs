using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Padstone.Data;

namespace Padstone.Tests
{
	[TestClass]
	public class CartesianTreeTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			CartesianTree<int> testTree = new CartesianTree<int>();
			testTree.AddRange(new int[] { 2, 4, 1, 0, 5, -3, 12 });
			Assert.AreEqual(0, testTree.GetRangeExtremum(0, 4));
			Assert.AreEqual(-3, testTree.GetRangeExtremum(0, 7));
		}
	}
}

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
    public class TreapTests
    {
        private static Treap<int> GetTestTreap() => new Treap<int>(randomPriorityProvider: new Random(0).Next); // we need predictable random number generation, if we're to reproduce test runs reliably

        [TestClass]
        public class ContainsTests
        {
            [TestMethod]
            public void ReturnsTrueIfTreapContainsItem()
            {
                Treap<int> treap = GetTestTreap();
                treap.Add(4);

                Assert.IsTrue(treap.Contains(4));
            }

            [TestMethod]
            public void ReturnsFalseIfItemIsMissing1()
            {
                Treap<int> treap = GetTestTreap();
                treap.Add(4);

                Assert.IsFalse(treap.Contains(325));
            }

            [TestMethod]
            public void ReturnsFalseIfItemIsMissing2()
            {
                Treap<int> treap = GetTestTreap();

                Assert.IsFalse(treap.Contains(325));
            }

            [TestMethod]
            public void ReturnsTrueIfTreapContainsItemAndMoreObjects()
            {
                var treap = GetTestTreap();
                treap.Add(4);
                treap.Add(12);
                treap.Add(5);
                treap.Add(7);
                treap.Add(0);
                treap.Add(1);
                treap.Add(2);

                Assert.IsTrue(treap.Contains(5));
            }
        }
    }
}

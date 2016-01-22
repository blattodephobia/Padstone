using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Padstone.Xaml.Tests
{
    [TestClass]
    public class PropertyNameTests
    {
        [TestClass]
        public class NameOfTests
        {
            static string StaticProperty { get; set; }

            const int Constant = 0;

            string TestProperty { get; set; }

            string testField = "";

            int TestValueTypeProperty { get; set; }

            [TestMethod]
            public void ReturnsInstanceMemberName()
            {
                string actual = PropertyName.Get(() => this.TestProperty);
                Assert.AreEqual("TestProperty", actual);
            }

            [TestMethod]
            public void ReturnsInstanceMemberNameWithBoxing()
            {
                string actual = PropertyName.Get<object>(() => this.TestValueTypeProperty);
                Assert.AreEqual("TestValueTypeProperty", actual);
            }

            [TestMethod]
            public void ReturnsInstanceFieldName()
            {
                string actual = PropertyName.Get(() => this.testField);
                Assert.AreEqual("testField", actual);
            }

            [TestMethod]
            public void ReturnsNonInstantiatedMemberName()
            {
                string actual = PropertyName.Get<NameOfTests>((obj) => obj.TestProperty);
                Assert.AreEqual("TestProperty", actual);
            }

            [TestMethod]
            public void ReturnsNonInstantiatedMemberNameWithBoxing()
            {
                string actual = PropertyName.Get<NameOfTests>((obj) => obj.TestValueTypeProperty);
                Assert.AreEqual("TestValueTypeProperty", actual);
            }

            [TestMethod]
            public void ReturnsNonInstantiatedFieldName()
            {
                string actual = PropertyName.Get<NameOfTests>((obj) => obj.testField);
                Assert.AreEqual("testField", actual);
            }

            [TestMethod]
            public void ReturnsStaticPropertyName()
            {
                string actual = PropertyName.Get(() => StaticProperty);
                Assert.AreEqual("StaticProperty", actual);
                actual = PropertyName.Get(() => NameOfTests.StaticProperty);
                Assert.AreEqual("StaticProperty", actual);
            }

            [TestMethod]
            public void ReturnsDependencyPropertyName()
            {
                DependencyProperty testProperty = null;
                string actual = PropertyName.FromDependencyProperty(() => testProperty);
                Assert.AreEqual("test", actual);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsOnConstantExpression()
            {
                string actual = PropertyName.Get(() => Constant);
                Assert.AreEqual("Constant", actual);
                actual = PropertyName.Get(() => NameOfTests.Constant);
                Assert.AreEqual("Constant", actual);
            }
        }
    }
}

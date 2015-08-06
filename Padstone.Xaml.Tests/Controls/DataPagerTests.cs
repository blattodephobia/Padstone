using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Padstone.Xaml.Controls;

namespace Padstone.Xaml.Tests.Controls
{
    [TestClass]
    public class DataPagerTests
    {
        public static IEnumerable<int> GetMockCollection(int itemsCount)
        {
            return Enumerable.Range(0, itemsCount).ToList();
        }

        [TestClass]
        public class PageLinksTests
        {

            [TestMethod]
            public void GeneratesCorrectNumberOfPages1()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 15;
                IEnumerable<int> items = GetMockCollection(20);
                pager.ItemsSource = items;

                Assert.AreEqual(2, pager.PageLinks.Count);
            }

            [TestMethod]
            public void GeneratesCorrectNumberOfPages2()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 15;
                IEnumerable<int> items = GetMockCollection(10);
                pager.ItemsSource = items;

                Assert.AreEqual(1, pager.PageLinks.Count);
            }

            [TestMethod]
            public void GeneratesCorrectNumberOfPages3()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 1;
                IEnumerable<int> items = GetMockCollection(10);
                pager.ItemsSource = items;

                Assert.AreEqual(10, pager.PageLinks.Count);
            }

            [TestMethod]
            public void GeneratesCorrectNumberOfPages4()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;
                IEnumerable<int> items = GetMockCollection(20);
                pager.ItemsSource = items;

                Assert.AreEqual(2, pager.PageLinks.Count);
            }
        }

        [TestClass]
        public class PageItemsTests
        {
            [TestMethod]
            public void GeneratesCorrectPageItems1()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;
                IEnumerable<int> items = GetMockCollection(20);
                pager.ItemsSource = items;

                Assert.AreEqual(0, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());
            }

            [TestMethod]
            public void GeneratesCorrectPageItems2()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;
                IEnumerable<int> items = GetMockCollection(15);
                pager.ItemsSource = items;
                pager.CurrentPage = new PageLink(1);

                Assert.AreEqual(10, pager.PageItems.First());
                Assert.AreEqual(14, pager.PageItems.Last());
            }
        }

        [TestClass]
        public class CommandsTests
        {
            #region Next page command

            [TestMethod]
            public void NavigatesToNextPage1()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;

                IEnumerable<int> items = GetMockCollection(15);
                pager.ItemsSource = items;
                Assert.AreEqual(0, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());
                
                pager.NavigateToNextPageCommand.Execute(null);
                Assert.AreEqual(1, pager.CurrentPage.PageIndex);
                Assert.AreEqual(10, pager.PageItems.First());
                Assert.AreEqual(14, pager.PageItems.Last());

                pager.NavigateToNextPageCommand.Execute(null);
                Assert.AreEqual(1, pager.CurrentPage.PageIndex);
                Assert.AreEqual(10, pager.PageItems.First());
                Assert.AreEqual(14, pager.PageItems.Last());
            }

            [TestMethod]
            public void NavigatesToNextPage2()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;

                IEnumerable<int> items = GetMockCollection(25);
                pager.ItemsSource = items;
                Assert.AreEqual(0, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());

                pager.NavigateToNextPageCommand.Execute(null);
                Assert.AreEqual(1, pager.CurrentPage.PageIndex);
                Assert.AreEqual(10, pager.PageItems.First());
                Assert.AreEqual(19, pager.PageItems.Last());

                pager.NavigateToNextPageCommand.Execute(null);
                Assert.AreEqual(2, pager.CurrentPage.PageIndex);
                Assert.AreEqual(20, pager.PageItems.First());
                Assert.AreEqual(24, pager.PageItems.Last());
            }

            #endregion

            #region Previous page command

            [TestMethod]
            public void NavigatesToPreviousPage1()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;

                IEnumerable<int> items = GetMockCollection(15);
                pager.ItemsSource = items;
                pager.CurrentPage = new PageLink(1);
                Assert.AreEqual(10, pager.PageItems.First());
                Assert.AreEqual(14, pager.PageItems.Last());
                
                pager.NavigateToPreviousPageCommand.Execute(null);
                Assert.AreEqual(0, pager.CurrentPage.PageIndex);
                Assert.AreEqual(0, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());

                pager.NavigateToPreviousPageCommand.Execute(null);
                Assert.AreEqual(0, pager.CurrentPage.PageIndex);
                Assert.AreEqual(0, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());
            }

            [TestMethod]
            public void NavigatesToPreviousPage2()
            {
                DataPager pager = new DataPager();
                pager.PageSize = 10;

                IEnumerable<int> items = GetMockCollection(25);
                pager.ItemsSource = items;
                pager.CurrentPage = new PageLink(2);
                Assert.AreEqual(2, pager.CurrentPage.PageIndex);
                Assert.AreEqual(20, pager.PageItems.First());
                Assert.AreEqual(24, pager.PageItems.Last());

                pager.NavigateToPreviousPageCommand.Execute(null);
                Assert.AreEqual(1, pager.CurrentPage.PageIndex);
                Assert.AreEqual(10, pager.PageItems.First());
                Assert.AreEqual(19, pager.PageItems.Last());

                pager.NavigateToPreviousPageCommand.Execute(null);
                Assert.AreEqual(0, pager.CurrentPage.PageIndex);
                Assert.AreEqual(0, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());
            }

            #endregion

            #region Navigate to page command

            [TestMethod]
            public void TestNavigateToPageCommand()
            {
                DataPager pager = new DataPager();
                pager.ItemsSource = GetMockCollection(30);
                pager.PageSize = 5;

                pager.NavigateToPageCommand.Execute(new PageLink(3));
                Assert.AreEqual(4, pager.CurrentPage.PageNumber);
                Assert.AreEqual(15, pager.PageItems.First());
                Assert.AreEqual(19, pager.PageItems.Last());

                pager.NavigateToPageCommand.Execute(new PageLink(1));
                Assert.AreEqual(1, pager.CurrentPage.PageIndex);
                Assert.AreEqual(5, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());

                pager.NavigateToPageCommand.Execute(new PageLink(5));
                Assert.AreEqual(6, pager.CurrentPage.PageNumber);
                Assert.AreEqual(25, pager.PageItems.First());
                Assert.AreEqual(29, pager.PageItems.Last());
            }

            [TestMethod]
            public void TestNavigateToPageCommandWithInvalidIndices()
            {
                DataPager pager = new DataPager();
                pager.ItemsSource = GetMockCollection(30);
                pager.PageSize = 5;

                pager.NavigateToPageCommand.Execute(new PageLink(1));
                Assert.AreEqual(2, pager.CurrentPage.PageNumber);
                Assert.AreEqual(5, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());

                pager.NavigateToPageCommand.Execute(new PageLink(10));
                Assert.AreEqual(2, pager.CurrentPage.PageNumber);
                Assert.AreEqual(5, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());

                pager.NavigateToPageCommand.Execute(new PageLink(-1));
                Assert.AreEqual(2, pager.CurrentPage.PageNumber);
                Assert.AreEqual(5, pager.PageItems.First());
                Assert.AreEqual(9, pager.PageItems.Last());
            }

            #endregion
        }
    }
}

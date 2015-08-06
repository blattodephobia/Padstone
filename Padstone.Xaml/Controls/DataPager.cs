using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Padstone.Xaml.Controls
{
    public class DataPager : ItemsControl
    {
        private static readonly int DefaultPageSize = 15;

        private static readonly PageLink DefaultPage = new PageLink(0);

        #region DependencyProperty declarations

        public static readonly DependencyProperty PageItemsProperty;
        protected static readonly DependencyPropertyKey PageItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            PropertyName.FromDependencyProperty(() => PageItemsProperty),
            typeof(ObservableCollection<object>),
            typeof(DataPager),
            new PropertyMetadata(
                new ObservableCollection<object>(),
                PageItemsPropertyChanged));

        public static readonly DependencyProperty PageLinksProperty;
        protected static readonly DependencyPropertyKey PageLinksPropertyKey = DependencyProperty.RegisterReadOnly(
            PropertyName.FromDependencyProperty(() => PageLinksProperty),
            typeof(ObservableCollection<PageLink>),
            typeof(DataPager),
            new PropertyMetadata(
                new ObservableCollection<PageLink>(),
                PageLinksPropertyChanged));

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
            PropertyName.FromDependencyProperty(() => PageSizeProperty),
            typeof(int),
            typeof(DataPager),
            new PropertyMetadata(
                DefaultPageSize,
                PageSizePropertyChanged,
                CoercePageSizeCallback));
        
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            PropertyName.FromDependencyProperty(() => CurrentPageProperty),
            typeof(PageLink),
            typeof(DataPager),
            new PropertyMetadata(
                DefaultPage,
                CurrentPagePropertyChanged,
                CoerceCurrentPageCallback));
        
        #endregion

        #region Static helper methods

        internal static IList<PageLink> CalculatePageLinks(IEnumerable source, int pageSize, int firstPageIndex)
        {
            int items = source.Cast<object>().Count();
            int pagesCount = (int)Math.Ceiling(items / (double)pageSize);

            List<PageLink> result = Enumerable.Range(firstPageIndex, pagesCount).Select(index => new PageLink(index)).ToList();
            return result;
        }

        #endregion

        #region DependencyProperty handlers

        private static void PageItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataPager).pageItems = e.NewValue as ObservableCollection<object>;
        }

        private static void PageLinksPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataPager).pageLinks = e.NewValue as ObservableCollection<PageLink>;
        }

        private static void PageSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPager pager = d as DataPager;
            pager.pageSize = (int)e.NewValue;
            pager.OnPageSizeChanged((int)e.OldValue, pager.pageSize);
        }

        private static object CoercePageSizeCallback(DependencyObject d, object baseValue)
        {
            int minValue = 1;
            return minValue.CompareTo((int)baseValue) > 0 ? minValue : baseValue;
        }

        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPager sender = d as DataPager;
            sender.currentPage = e.NewValue as PageLink;
            sender.OnCurrentPageChanged(e.OldValue as PageLink, e.NewValue as PageLink);
        }

        private static object CoerceCurrentPageCallback(DependencyObject d, object baseValue)
        {
            PageLink currentValue = baseValue as PageLink;
            int minIndex = 0;
            int maxIndex = (d as DataPager).PageLinks.Count - 1;

            PageLink result = currentValue != null && minIndex.CompareTo(currentValue.PageIndex) > 0 ? DefaultPage : baseValue as PageLink;
            result = maxIndex.CompareTo(result.PageIndex) < 0 ? new PageLink(Math.Max(0, maxIndex)) : result;
            return result;
        }

        #endregion

        static DataPager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPager), new FrameworkPropertyMetadata(typeof(DataPager)));
            PageItemsProperty = PageItemsPropertyKey.DependencyProperty;
            PageLinksProperty = PageLinksPropertyKey.DependencyProperty;
        }

        private ObservableCollection<object> pageItems;
        public ObservableCollection<object> PageItems
        {
            get
            {
                return this.pageItems;
            }

            private set
            {
                this.SetValue(PageItemsPropertyKey, value);
            }
        }

        private PageLink currentPage;
        public PageLink CurrentPage
        {
            get
            {
                return this.currentPage;
            }

            set
            {
                this.SetValue(CurrentPageProperty, value);
            }
        }

        private ObservableCollection<PageLink> pageLinks;
        public ObservableCollection<PageLink> PageLinks
        {
            get
            {
                return this.pageLinks;
            }

            private set
            {
                this.SetValue(PageLinksPropertyKey, value);
            }
        }

        private int pageSize;
        public int PageSize
        {
            get
            {
                return this.pageSize;
            }

            set
            {
                this.SetValue(PageSizeProperty, value);
            }
        }

        public ICommand NavigateToPreviousPageCommand
        {
            get
            {
                return this.navigateToPreviousPageCommand;
            }
        }

        public ICommand NavigateToNextPageCommand
        {
            get
            {
                return this.navigateToNextPageCommand;
            }
        }

        public ICommand NavigateToPageCommand
        {
            get
            {
                return this.navigateToPageCommand;
            }
        }

        private DelegateCommand navigateToPreviousPageCommand;
        protected DelegateCommand NavigateToPreviousPageCommandInternal
        {
            get
            {
                return this.navigateToPreviousPageCommand;
            }

            set
            {
                this.navigateToPreviousPageCommand = value;
            }
        }

        private DelegateCommand navigateToNextPageCommand;
        protected DelegateCommand NavigateToNextPageCommandInternal
        {
            get
            {
                return this.navigateToNextPageCommand;
            }

            set
            {
                this.navigateToNextPageCommand = value;
            }
        }

        private DelegateCommand navigateToPageCommand;
        protected DelegateCommand NavigateToPageCommandInternal
        {
            get
            {
                return this.navigateToPageCommand;
            }

            set
            {
                this.navigateToPageCommand = value;
            }
        }

        public DataPager()
        {
            this.PageLinks = new ObservableCollection<PageLink>();
            this.PageItems = new ObservableCollection<object>();

            this.pageSize = DefaultPageSize;
            this.currentPage = new PageLink(0);

            this.NavigateToPreviousPageCommandInternal = new DelegateCommand(
                this.NavigateToPreviousPage,
                this.CanNavigateToPreviousPage);

            this.NavigateToNextPageCommandInternal = new DelegateCommand(
                this.NavigateToNextPage,
                this.CanNavigateToNextPage);

            this.NavigateToPageCommandInternal = new DelegateCommand(
                this.NavigateToPage,
                this.CanNavigateToPageAt);
        }

        protected virtual bool CanNavigateToPageAt(object pageLink)
        {
            return this.PageLinks.Any(p => p.PageIndex == (pageLink as PageLink).PageIndex);
        }

        protected virtual bool CanNavigateToNextPage()
        {
            return
                this.CurrentPage != null &&
                this.PageLinks.Any() &&
                this.CurrentPage.PageIndex < this.PageLinks.Last().PageIndex;
        }

        protected virtual bool CanNavigateToPreviousPage()
        {
            return
                this.CurrentPage != null &&
                this.PageLinks.Any() &&
                this.CurrentPage.PageIndex > this.PageLinks.First().PageIndex;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            this.PerformPagesReflow(this.ItemsSource);
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.PerformPagesReflow(this.ItemsSource);
        }

        protected virtual void OnPageSizeChanged(int oldValue, int newValue)
        {
            this.PerformPagesReflow(this.ItemsSource);
            this.UpdatePageItems();
        }

        protected virtual void OnCurrentPageChanged(PageLink oldValue, PageLink newValue)
        {
            this.UpdatePageItems();
        }

        protected void NavigateToPreviousPage(object dummy)
        {
            if (this.CanNavigateToPreviousPage())
            {
                this.CurrentPage = this.PageLinks.First(p => p.PageIndex == this.CurrentPage.PageIndex - 1);
            }
        }

        protected void NavigateToNextPage(object dummy)
        {
            if (this.CanNavigateToNextPage())
            {
                this.CurrentPage = this.PageLinks.First(p => p.PageIndex == this.CurrentPage.PageIndex + 1);
            }
        }

        protected void NavigateToPage(object page)
        {
            if (this.CanNavigateToPageAt(page))
            {
                this.CurrentPage = page as PageLink;
            }
        }

        /// <summary>
        /// Causes the page links and the visible page items to be recalculated.
        /// </summary>
        protected void PerformPagesReflow(IEnumerable source)
        {
            if (source != null)
            {
                int firstPageIndex = this.CurrentPage != null ? this.CurrentPage.PageIndex : 0;
                IList<PageLink> newLinks = CalculatePageLinks(source, this.PageSize, firstPageIndex);
                if (newLinks.Count == this.PageLinks.Count)
                {
                    for (int i = 0; i < newLinks.Count; i++)
                    {
                        this.PageLinks[i] = newLinks[i];
                    }
                }
                else
                {
                    this.PageLinks.Clear();
                    foreach (PageLink link in newLinks)
                    {
                        this.PageLinks.Add(link);
                    }
                }

                this.CurrentPage = this.PageLinks.First(p => p.PageIndex == firstPageIndex);
            }
        }

        protected void UpdatePageItems()
        {
            if (this.ItemsSource != null)
            {
                PageLink currentPage = this.CurrentPage ?? new PageLink(0);
                List<object> currentPageItems = this.ItemsSource
                    .Cast<object>()
                    .Skip(this.PageSize * currentPage.PageIndex)
                    .Take(this.PageSize).ToList();
                if (currentPageItems.Count == this.PageItems.Count)
                {
                    for (int i = 0; i < currentPageItems.Count; i++)
                    {
                        this.PageItems[i] = currentPageItems[i];
                    }
                }
                else
                {
                    this.PageItems.Clear();
                    foreach (object item in currentPageItems)
                    {
                        this.PageItems.Add(item);
                    }
                }

                this.InvalidateCommands();
            }
        }

        private void InvalidateCommands()
        {
            this.NavigateToNextPageCommandInternal.InvalidateCanExecute();
            this.NavigateToPageCommandInternal.InvalidateCanExecute();
            this.NavigateToPreviousPageCommandInternal.InvalidateCanExecute();
        }
    }
}

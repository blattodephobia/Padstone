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
    public class DataPager : Control
    {
        private static readonly int DefaultPageSize = 15;

        #region DependencyProperty declarations

        protected static readonly DependencyPropertyKey CurrentPageItemsPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.CurrentPageItems),
            typeof(ObservableCollection<object>),
            typeof(DataPager),
            new PropertyMetadata(
                new ObservableCollection<object>(),
                PageItemsPropertyChanged));
        public static readonly DependencyProperty CurrentPageItemsProperty = CurrentPageItemsPropertyKey.DependencyProperty;

        protected static readonly DependencyPropertyKey PageLinksPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.PageLinks),
            typeof(ObservableCollection<PageLink>),
            typeof(DataPager),
            new PropertyMetadata(
                new ObservableCollection<PageLink>(),
				PageLinksPropertyChanged));
		public static readonly DependencyProperty PageLinksProperty = PageLinksPropertyKey.DependencyProperty;

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.PageSize),
            typeof(int),
            typeof(DataPager),
            new PropertyMetadata(
                DefaultPageSize,
                PageSizePropertyChanged,
                CoercePageSizeCallback));
        
        public static readonly DependencyProperty CurrentPageIndexProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.CurrentPageIndex),
            typeof(int),
            typeof(DataPager),
            new PropertyMetadata(
                0,
                CurrentPagePropertyChanged,
                CoerceCurrentPageCallback));
		
		protected static readonly DependencyPropertyKey NavigateToPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.NavigateToPageCommand),
			typeof(ICommand),
			typeof(DataPager),
			new PropertyMetadata(
				null,
				NavigateToPageCommandPropertyChanged));
		public static readonly DependencyProperty NavigateToPageCommandProperty = NavigateToPageCommandPropertyKey.DependencyProperty;

        protected static readonly DependencyPropertyKey NavigateToPreviousPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.NavigateToPreviousPageCommand),
			typeof(ICommand),
			typeof(DataPager),
			new PropertyMetadata(
				null,
				NavigateToPreviousPageCommandPropertyChanged));
		public static readonly DependencyProperty NavigateToPreviousPageCommandProperty = NavigateToPreviousPageCommandPropertyKey.DependencyProperty;

        protected static readonly DependencyPropertyKey NavigateToNextPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.NavigateToNextPageCommand),
			typeof(ICommand),
			typeof(DataPager),
			new PropertyMetadata(
				null,
				NavigateToNextPageCommandPropertyChanged));
		public static readonly DependencyProperty NavigateToNextPageCommandProperty = NavigateToNextPageCommandPropertyKey.DependencyProperty;

		protected static readonly DependencyPropertyKey NavigateToFirstPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.NavigateToFirstPageCommand),
			typeof(ICommand),
			typeof(DataPager),
			new PropertyMetadata(
				null,
				NavigateToFirstPageCommandPropertyChanged));
		public static readonly DependencyProperty NavigateToFirstPageCommandProperty = NavigateToFirstPageCommandPropertyKey.DependencyProperty;

		protected static readonly DependencyPropertyKey NavigateToLastPageCommandPropertyKey = DependencyProperty.RegisterReadOnly(
			PropertyName.Get<DataPager>(p => p.NavigateToLastPageCommand),
			typeof(ICommand),
			typeof(DataPager),
			new PropertyMetadata(
				null,
				NavigateToLastPageCommandPropertyChanged));

		public static readonly DependencyProperty NavigateToLastPageCommandProperty = NavigateToLastPageCommandPropertyKey.DependencyProperty;

		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.Orientation),
			typeof(Orientation),
			typeof(DataPager),
			new PropertyMetadata(
				Orientation.Horizontal,
				OrientationPropertyChanged));
		
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.ItemsSource),
			typeof(IEnumerable),
			typeof(DataPager),
			new PropertyMetadata(
				null,
				ItemsSourcePropertyChanged));

		public static readonly DependencyProperty MaxTrailingPageLinksProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.MaxTrailingPageLinks),
			typeof(int),
			typeof(DataPager),
			new PropertyMetadata(
				3,
				MaxTrailingPageLinksPropertyChanged,
				CoerceMaxTrailingPageLinksCallback));

		public static readonly DependencyProperty MaxMainPageLinksProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.MaxMainPageLinks),
			typeof(int),
			typeof(DataPager),
			new PropertyMetadata(
				8,
				MaxMainPageLinksPropertyChanged,
				CoerceMaxMainPageLinksCallback));

		public static readonly DependencyProperty NavigateToFirstPageElementTemplateProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToFirstPageElementTemplate),
			typeof(DataTemplate),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToPreviousPageElementTemplateProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToPreviousPageElementTemplate),
			typeof(DataTemplate),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty PageLinkTemplateProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.PageLinkTemplate),
			typeof(DataTemplate),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToNextPageElementTemplateProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToNextPageElementTemplate),
			typeof(DataTemplate),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToLastPageElementTemplateProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToLastPageElementTemplate),
			typeof(DataTemplate),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToFirstPageElementStyleProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToFirstPageElementStyle),
			typeof(Style),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToPreviousPageElementStyleProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToPreviousPageElementStyle),
			typeof(Style),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty PageLinkElementStyleProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.PageLinkElementStyle),
			typeof(Style),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToNextPageElementStyleProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToNextPageElementStyle),
			typeof(Style),
			typeof(DataPager),
			new PropertyMetadata(null));

		public static readonly DependencyProperty NavigateToLastPageElementStyleProperty = DependencyProperty.Register(
			PropertyName.Get<DataPager>(p => p.NavigateToLastPageElementStyle),
			typeof(Style),
			typeof(DataPager),
			new PropertyMetadata(null));

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
		private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataPager _obj = d as DataPager;
			_obj.itemsSource = e.NewValue as IEnumerable;
			_obj.OnItemsSourceChanged(e.OldValue as IEnumerable, e.NewValue  as IEnumerable);

			INotifyCollectionChanged _observableOld = e.OldValue as INotifyCollectionChanged;
			if (_observableOld != null) _observableOld.CollectionChanged -= _obj.INotifyCollectionChangedEventHandler;

			INotifyCollectionChanged _observableNew = e.NewValue as INotifyCollectionChanged;
			if (_observableNew != null) _observableNew.CollectionChanged += _obj.INotifyCollectionChangedEventHandler;
		}

        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataPager).orientation = OrientationBox.Unbox(e.NewValue);
        }

        private static void NavigateToPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataPager).navigateToPageCommand = e.NewValue as DelegateCommand;
        }

		private static void NavigateToPreviousPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			(d as DataPager).navigateToPreviousPageCommand = e.NewValue as DelegateCommand;
        }

		private static void NavigateToNextPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			(d as DataPager).navigateToNextPageCommand = e.NewValue as DelegateCommand;
        }

		private static void NavigateToFirstPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DataPager).navigateToFirstPageCommand = e.NewValue as DelegateCommand;
		}

		private static void NavigateToLastPageCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DataPager).navigateToLastPageCommand = e.NewValue as DelegateCommand;
		}

        private static void PageItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataPager).currentPageItems = e.NewValue as ObservableCollection<object>;
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

		private static void MaxTrailingPageLinksPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataPager sender = d as DataPager;
			sender.maxTrailingPageLinks = (int)e.NewValue;
			sender.UpdatePageLinks(sender.ItemsSource);
			sender.UpdateCurrentPageItems();
		}

		private static void MaxMainPageLinksPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataPager sender = d as DataPager;
			sender.maxMainPageLinks = (int)e.NewValue;
			sender.UpdatePageLinks(sender.ItemsSource);
			sender.UpdateCurrentPageItems();
		}

        private static object CoercePageSizeCallback(DependencyObject d, object baseValue)
        {
            int minValue = 1;
            return minValue.CompareTo((int)baseValue) > 0 ? minValue : baseValue;
        }

        private static object CoerceMaxTrailingPageLinksCallback(DependencyObject d, object baseValue)
		{
			return Math.Max((int)baseValue, 0);
		}

        private static object CoerceMaxMainPageLinksCallback(DependencyObject d, object baseValue)
		{
			return Math.Max((int)baseValue, 1);
		}

        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPager sender = d as DataPager;
			sender.currentPageIndex = (int)e.NewValue;
            sender.OnCurrentPageChanged(sender.PageLinks[(int)e.OldValue], sender.PageLinks[(int)e.NewValue]);
        }

        private static object CoerceCurrentPageCallback(DependencyObject d, object baseValue)
        {
			int currentValue = (int)baseValue;
            int minIndex = 0;
            int maxIndex = (d as DataPager).PageLinks.Count - 1;

            int result = minIndex.CompareTo(currentValue) > 0 ? 0 : currentValue;
            result = maxIndex.CompareTo(result) < 0 ? Math.Max(0, maxIndex) : result;
            return result;
        }

        #endregion

        static DataPager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPager), new FrameworkPropertyMetadata(typeof(DataPager)));
        }
		
		#region Properties
		
		public DataTemplate NavigateToFirstPageElementTemplate
		{
			get { return (DataTemplate)this.GetValue(NavigateToFirstPageElementTemplateProperty); }
			set { this.SetValue(NavigateToFirstPageElementTemplateProperty, value); }
		}

		public DataTemplate NavigateToPreviousPageElementTemplate
		{
			get { return (DataTemplate)this.GetValue(NavigateToPreviousPageElementTemplateProperty); }
			set { this.SetValue(NavigateToPreviousPageElementTemplateProperty, value); }
		}

		public DataTemplate PageLinkTemplate
		{
			get { return (DataTemplate)this.GetValue(PageLinkTemplateProperty); }
			set { this.SetValue(PageLinkTemplateProperty, value); }
		}

		public DataTemplate NavigateToNextPageElementTemplate
		{
			get { return (DataTemplate)this.GetValue(NavigateToNextPageElementTemplateProperty); }
			set { this.SetValue(NavigateToNextPageElementTemplateProperty, value); }
		}

		public DataTemplate NavigateToLastPageElementTemplate
		{
			get { return (DataTemplate)this.GetValue(NavigateToLastPageElementTemplateProperty); }
			set { this.SetValue(NavigateToLastPageElementTemplateProperty, value); }
		}

		public Style NavigateToFirstPageElementStyle
		{
			get { return (Style)this.GetValue(NavigateToFirstPageElementStyleProperty); }
			set { this.SetValue(NavigateToFirstPageElementStyleProperty, value); }
		}
		
		public Style NavigateToPreviousPageElementStyle
		{
			get { return (Style)this.GetValue(NavigateToPreviousPageElementStyleProperty); }
			set { this.SetValue(NavigateToPreviousPageElementStyleProperty, value); }
		}
		
		public Style PageLinkElementStyle
		{
			get { return (Style)this.GetValue(PageLinkElementStyleProperty); }
			set { this.SetValue(PageLinkElementStyleProperty, value); }
		}
				
		public Style NavigateToNextPageElementStyle
		{
			get { return (Style)this.GetValue(NavigateToNextPageElementStyleProperty); }
			set { this.SetValue(NavigateToNextPageElementStyleProperty, value); }
		}

		public Style NavigateToLastPageElementStyle
		{
			get { return (Style)this.GetValue(NavigateToLastPageElementStyleProperty); }
			set { this.SetValue(NavigateToLastPageElementStyleProperty, value); }
		}

		private ObservableCollection<object> currentPageItems;
        public ObservableCollection<object> CurrentPageItems
        {
            get
            {
                return this.currentPageItems;
            }

            private set
            {
                this.SetValue(CurrentPageItemsPropertyKey, value);
            }
		}

		private int maxTrailingPageLinks;
		public int MaxTrailingPageLinks
		{
			get
			{
				return this.maxTrailingPageLinks;
			}

			set
			{
				this.SetValue(MaxTrailingPageLinksProperty, value);
			}
		}

		private int maxMainPageLinks;
		public int MaxMainPageLinks
		{
			get
			{
				return this.maxMainPageLinks;
			}

			set
			{
				this.SetValue(MaxMainPageLinksProperty, value);
			}
		}

		private Orientation orientation;
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}

			set
			{
				this.SetValue(OrientationProperty, OrientationBox.Box(value));
			}
		}

        private int currentPageIndex;
        public int CurrentPageIndex
        {
            get
            {
                return this.currentPageIndex;
            }

            set
            {
                this.SetValue(CurrentPageIndexProperty, value);
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

		public ICommand NavigateToFirstPageCommand
		{
			get
			{
				return this.navigateToFirstPageCommand;
			}
		}

		public ICommand NavigateToLastPageCommand
		{
			get
			{
				return this.navigateToLastPageCommand;
			}
		}

		private IEnumerable itemsSource;
		public IEnumerable ItemsSource
		{
			get
			{
				return this.itemsSource;
			}

			set
			{
				this.SetValue(ItemsSourceProperty, value);
			}
		}

		public override void BeginInit()
		{
			base.BeginInit();
			this.IsInitializing = true;
		}

		public override void EndInit()
		{
			base.EndInit();
			this.IsInitializing = false;
		}

		protected bool IsInitializing { get; set; }

        private DelegateCommand navigateToPreviousPageCommand;
        protected DelegateCommand NavigateToPreviousPageCommandInternal
        {
            get
            {
                return this.navigateToPreviousPageCommand;
            }

            set
            {
				this.SetValue(NavigateToPreviousPageCommandPropertyKey, value);
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
				this.SetValue(NavigateToNextPageCommandPropertyKey, value);
            }
        }

		private DelegateCommand navigateToFirstPageCommand;
		protected DelegateCommand NavigateToFirstPageCommandInternal
		{
			get
			{
				return this.navigateToFirstPageCommand;
			}

			set
			{
				this.SetValue(NavigateToFirstPageCommandPropertyKey, value);
			}
		}

		private DelegateCommand navigateToLastPageCommand;
		protected DelegateCommand NavigateToLastPageCommandInternal
		{
			get
			{
				return this.navigateToLastPageCommand;
			}

			set
			{
				this.SetValue(NavigateToLastPageCommandPropertyKey, value);
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
				this.SetValue(NavigateToPageCommandPropertyKey, value);
            }
        }

		#endregion

		public DataPager()
        {
            this.PageLinks = new ObservableCollection<PageLink>();
            this.CurrentPageItems = new ObservableCollection<object>();

			this.pageSize = DefaultPageSize;

            this.NavigateToPreviousPageCommandInternal = new DelegateCommand(
                this.NavigateToPreviousPage,
                this.CanNavigateToPreviousPage);

            this.NavigateToNextPageCommandInternal = new DelegateCommand(
                this.NavigateToNextPage,
                this.CanNavigateToNextPage);

            this.NavigateToPageCommandInternal = new DelegateCommand(
                this.NavigateToPage);

			this.NavigateToFirstPageCommandInternal = new DelegateCommand(
				this.NavigateToFirstPage,
				this.CanNavigateToFirstPage);

			this.NavigateToLastPageCommandInternal = new DelegateCommand(
				this.NavigateToLastPage,
				this.CanNavigateToLastPage);
        }

        protected virtual bool CanNavigateToNextPage()
        {
            return
				this.PageLinks.Any() &&
                this.CurrentPageIndex < this.PageLinks.Last().PageIndex;
        }

        protected virtual bool CanNavigateToPreviousPage()
        {
            return
                this.PageLinks.Any() &&
                this.CurrentPageIndex > this.PageLinks.First().PageIndex;
        }

		protected virtual bool CanNavigateToFirstPage()
		{
			return this.CurrentPageIndex > 0;
		}

		protected virtual bool CanNavigateToLastPage()
		{
			return this.CurrentPageIndex < this.PageLinks.Count - 1;
		}

        protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            this.UpdatePageLinks(this.ItemsSource);
        }

        protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            this.UpdatePageLinks(this.ItemsSource);
			this.UpdateCurrentPageItems();
        }

        protected virtual void OnPageSizeChanged(int oldValue, int newValue)
        {
            this.UpdatePageLinks(this.ItemsSource);
            this.UpdateCurrentPageItems();
        }

        protected virtual void OnCurrentPageChanged(PageLink oldValue, PageLink newValue)
        {
            this.UpdateCurrentPageItems();
        }

		protected virtual void NavigateToFirstPage(object dummy)
		{
			this.NavigateToPage(this.PageLinks[0]);
		}

        protected virtual void NavigateToPreviousPage(object dummy)
        {
			this.NavigateToPage(this.PageLinks[this.CurrentPageIndex - 1]);
        }

		protected virtual void NavigateToPage(object pageLink)
        {
			PageLink targetPage = pageLink as PageLink;
			if (targetPage != null && targetPage.PageIndex >= 0)
			{
				this.CurrentPageIndex = Math.Min(targetPage.PageIndex, this.PageLinks.Count - 1);
			}
			else if (pageLink is int)
			{
				this.CurrentPageIndex = Math.Min((int)(pageLink), this.PageLinks.Count - 1);
			}
			else
			{
				throw new InvalidOperationException();
			}
        }

		protected virtual void NavigateToNextPage(object dummy)
        {
			this.NavigateToPage(this.PageLinks[this.CurrentPageIndex + 1]);
        }

		protected virtual void NavigateToLastPage(object dummy)
		{
			this.NavigateToPage(this.PageLinks[this.PageLinks.Count - 1]);
		}

        /// <summary>
        /// Causes the page links and the visible page items to be recalculated.
        /// </summary>
        protected void UpdatePageLinks(IEnumerable source)
        {
            if (source != null)
            {
                int firstPageIndex = this.CurrentPageIndex != 0 ? this.CurrentPageIndex : 0;
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

				this.NavigateToFirstPage(this.PageLinks[firstPageIndex]);
            }
        }

        protected void UpdateCurrentPageItems()
        {
            if (this.ItemsSource != null)
            {
                List<object> currentPageItems = this.ItemsSource
                    .Cast<object>()
					.Skip(this.PageSize * this.CurrentPageIndex)
                    .Take(this.PageSize).ToList();
                if (currentPageItems.Count == this.CurrentPageItems.Count)
                {
                    for (int i = 0; i < currentPageItems.Count; i++)
                    {
                        this.CurrentPageItems[i] = currentPageItems[i];
                    }
                }
                else
                {
                    this.CurrentPageItems.Clear();
                    foreach (object item in currentPageItems)
                    {
                        this.CurrentPageItems.Add(item);
                    }
                }

                this.InvalidateCommands();
            }
        }

        private void InvalidateCommands()
        {
			this.NavigateToFirstPageCommandInternal.InvalidateCanExecute();
            this.NavigateToPreviousPageCommandInternal.InvalidateCanExecute();
            this.NavigateToPageCommandInternal.InvalidateCanExecute();
            this.NavigateToNextPageCommandInternal.InvalidateCanExecute();
			this.NavigateToLastPageCommandInternal.InvalidateCanExecute();
        }

		private void INotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnItemsChanged(e);
		}
    }
}

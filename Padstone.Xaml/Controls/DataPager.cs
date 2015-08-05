using System;
using System.Collections;
using System.Collections.Generic;
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
        static DataPager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPager), new FrameworkPropertyMetadata(typeof(DataPager)));
        }
        
        public IEnumerable PageItems
        {
            get { return (IEnumerable)GetValue(PageItemsProperty); }
            set { SetValue(PageItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageItemsProperty =
            DependencyProperty.Register("PageItems", typeof(IEnumerable), typeof(DataPager), new PropertyMetadata(null));


    }
}

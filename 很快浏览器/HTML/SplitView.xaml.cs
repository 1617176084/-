using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HTML
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SplitView : Page
    {
        ObservableCollection<item> m_items = null;
        public SplitView()
        {
            this.InitializeComponent();
            m_items = new ObservableCollection<item>();
            lvPrev.ItemsSource = m_items;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           // base.OnNavigatedTo(e);
            m_items.Clear();
            // 添加项列表
            m_items.Add(new item { Text = "雪花", Uri = new Uri("ms-appx:///Assets/images/1.jpg") });
            m_items.Add(new item { Text = "风筝", Uri = new Uri("ms-appx:///Assets/images/2.jpg") });
            m_items.Add(new item { Text = "核桃", Uri = new Uri("ms-appx:///Assets/images/3.jpg") });
            m_items.Add(new item { Text = "小溪", Uri = new Uri("ms-appx:///Assets/images/4.jpg") });
            m_items.Add(new item { Text = "胡杨", Uri = new Uri("ms-appx:///Assets/images/5.jpg") });
            m_items.Add(new item { Text = "红梅", Uri = new Uri("ms-appx:///Assets/images/6.jpg") });

        }
        private void swichClick(object sender, RoutedEventArgs e)
        {
        //     split.IsPaneOpen = !split.IsPaneOpen;      
        }
        static bool isTrue = false;
        private void OnClick(object sender, RoutedEventArgs e)
        {
            isTrue = (isTrue == true) ? false : true; ;
            split.IsPaneOpen = isTrue;
        }

        private void lvPrev_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
           object obj =  lvPrev.SelectedItem;
        }

        private void lvPrev_ItemClick(object sender, ItemClickEventArgs e)
        {            
       
            item obj = lvPrev.SelectedItem as item;
            BitmapImage imags = new BitmapImage(obj.Uri);
            image.Source = imags;
        }

        private void lvPrev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            item obj = lvPrev.SelectedItem as item;
            BitmapImage imags = new BitmapImage(obj.Uri);
            image.Source = imags;
        }
    }
    public class item 
    {
        public Uri Uri { get; set; }
        public string Text { get; set; }
    }
}

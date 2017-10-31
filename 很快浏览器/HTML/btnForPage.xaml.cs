using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HTML
{
    public sealed partial class btnForPage : UserControl
    {
        public btnForPage()
        {
            this.InitializeComponent();
        }
        public delegate void closss();
        public  event closss delegatePageCloss;

        public delegate void pageSelect();
        public event pageSelect delegatepageSelect;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (delegatePageCloss != null)
            {
                delegatePageCloss();
            }
        }
        public void setTitleName(string _name)
        {
            this.textBlock.Text = _name;
        }

        private void btnForselect_Click(object sender, RoutedEventArgs e)
        {
            if (delegatepageSelect != null)
            {
                delegatepageSelect();
            }
        }
        public void setSelect()
        {
            backColor.Visibility = Visibility.Visible;
            
         
            Color co = new Windows.UI.Color();
            co.R = 0;
            co.G = 0;
            co.B = 0;
            co.A = 255;
            this.textBlock.Foreground = new SolidColorBrush(co);
            button.Foreground = new SolidColorBrush(co);
        }
        public void setNoSelect()
        {
            backColor.Visibility = Visibility.Collapsed;
            Color co = new Windows.UI.Color();
            co.R = 255;
            co.G = 255;
            co.B = 255;
            co.A = 255;
            this.textBlock.Foreground = new SolidColorBrush(co);
            button.Foreground = new SolidColorBrush(co);
        }
    }
}

using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;
using JP.Utils.Data;
using JP.Utils.UI;
using Windows.UI;
using Windows.UI.Popups;
using System.Collections.ObjectModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace HTML.views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingView : Page
    {
        private object name;
        ObservableCollection<ColorThemeModel> colors;
        public SettingView()
        {
            this.InitializeComponent();
           colors = new ObservableCollection<ColorThemeModel>();
            colors.Add(new ColorThemeModel("RED", "#F44336"));
            colors.Add(new ColorThemeModel("PINK", "#E91E63"));
            colors.Add(new ColorThemeModel("PURPLE", "#9C27B0"));
            colors.Add(new ColorThemeModel("DEEP PURPLE", "#673AB7"));
            colors.Add(new ColorThemeModel("INDIGO", "#3F51B5"));
            colors.Add(new ColorThemeModel("BLUE", "#2196F3"));
            colors.Add(new ColorThemeModel("LIGHT BLUE", "#03A9F4"));
            colors.Add(new ColorThemeModel("CYAN", "#00BCD4"));
            colors.Add(new ColorThemeModel("TEAL", "#009688"));
            colors.Add(new ColorThemeModel("GREEN", "#4CAF50"));
            colors.Add(new ColorThemeModel("LIGHT GREEN", "#8BC34A"));
            colors.Add(new ColorThemeModel("LIME", "#CDDC39"));
            colors.Add(new ColorThemeModel("YELLOW", "#FFEB3B"));
            colors.Add(new ColorThemeModel("AMBER", "#FFC107"));
            colors.Add(new ColorThemeModel("ORANGE", "#FF9800"));
            colors.Add(new ColorThemeModel("DEEP ORANGE", "#FF5722"));
            colors.Add(new ColorThemeModel("BROWN", "#795548"));
            colors.Add(new ColorThemeModel("GREY", "#9E9E9E"));
            colors.Add(new ColorThemeModel("BLUE GREY", "#607D8B"));
            gridView.ItemsSource = colors;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            SolidColorBrush brush = ((Button)sender).Background as SolidColorBrush;
            Color color = brush.Color;
          bool isOk = ColorTheme.settingTheme(color);;
            if (isOk)
            {
                //主题色设置成功，请重启本程序，才能使本次设置生效
                MessageDialog dialog = new MessageDialog(AppResources.GetString("ThemeSettingOk"));
                dialog.ShowAsync();
               
            }
            else {
              //  设置失败，请联系开发者进行修复此问题（BUG）
                MessageDialog dialog = new MessageDialog(AppResources.GetString("ThemeSettingFail"));
                dialog.ShowAsync();
            }
         

        }
        public class ColorThemeModel{
            public string name { get; set; }
          public string color { get; set; }
            public   ColorThemeModel(string _name,string _color) {
                name = _name;
                color = _color;
            }


    }
    }
}

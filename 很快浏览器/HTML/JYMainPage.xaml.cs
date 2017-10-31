using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using JP.Utils.UI;
using JP.Utils.Data;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel.Store;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HTML
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JYMainPage : Page
    {
        public JYMainPage()
        {
            this.InitializeComponent();

            if (!LocalSettingHelper.HasValue("colorTheme"))
            {
                // LocalSettingHelper.AddValue("colorTheme", "#CC212121");
                ColorTheme.settingTheme("#15a415"); 
            }
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = ColorTheme.getTheme().Color;
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 1;


            }
            var view = ApplicationView.GetForCurrentView();
            //view.TitleBar.BackgroundColor = ColorTheme.getTheme().Color;
            // active
            view.TitleBar.BackgroundColor = ColorTheme.getTheme().Color;
            view.TitleBar.ForegroundColor = Colors.White;

            // inactive
            
            view.TitleBar.InactiveBackgroundColor = ColorTheme.getTheme().Color;
            view.TitleBar.InactiveForegroundColor = Colors.Gray;
            // button
            view.TitleBar.ButtonBackgroundColor = ColorTheme.getTheme().Color;
            view.TitleBar.ButtonForegroundColor = Colors.White;

            view.TitleBar.ButtonHoverBackgroundColor = ColorTheme.getTheme().Color;
            view.TitleBar.ButtonHoverForegroundColor = Colors.White;

            view.TitleBar.ButtonPressedBackgroundColor = ColorTheme.getTheme().Color;
            view.TitleBar.ButtonPressedForegroundColor = Colors.White;

            view.TitleBar.ButtonInactiveBackgroundColor = ColorTheme.getTheme().Color;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;

             


            listView.Background =  ColorTheme.getTheme();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            setSplit();
            string name = Package.Current.DisplayName;
            // this.frame.Navigate(typeof(page.webPage));
            this.listView.SelectedIndex = 0;
            NotyficationCenter.getNoty().delegatmainPageBack += (int Index) => {
                this.listView.SelectedIndex = Index;
            };
         
            //ResourceContext rsContext = ResourceManager.Current.DefaultContext;
            //rsContext.Languages = new List<string>(new string[] { appLanguage });

            //ResourceLoader loader = new ResourceLoader();

            //this.textBlock.Text = loader.GetString("lishijilu");
            //     this.txtbText2.Text = loader.GetString("item2");
            //      this.txtbText3.Text = loader.GetString("item3");

        }

        void setSplit()
        {
            if (Window.Current.Bounds.Width < 480)
            {
                split.DisplayMode = SplitViewDisplayMode.Overlay;
            }
            else
            {
                split.DisplayMode = SplitViewDisplayMode.CompactOverlay;
            }
      
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            split.IsPaneOpen = !split.IsPaneOpen;
            setSplit();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            split.IsPaneOpen = false;
            if (listView.SelectedIndex == 0)
            {
                Debug.WriteLine("首页");
                this.frame.Navigate(typeof(page.webPage));
            }
            if (listView.SelectedIndex == 1)
            {
                Debug.WriteLine("收藏夹");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("type", "shuQian");
             
                this.frame.Navigate(typeof(page.shouCangPage), dic);

            }
            if (listView.SelectedIndex == 2)
            {
                Debug.WriteLine("历史纪录");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("type", "lishi");

                this.frame.Navigate(typeof(page.shouCangPage), dic);
            }
            if (listView.SelectedIndex == 3)
            {
                Debug.WriteLine("下载");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("type", "xiazai");
                this.frame.Navigate(typeof(page.shouCangPage), dic);

            }
            if (listView.SelectedIndex == 4)
            {
                Debug.WriteLine("设置");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("type", "shezhi");
                this.frame.Navigate(typeof(page.shouCangPage), dic);
            }
            if (listView.SelectedIndex == 5)
            {
                Debug.WriteLine("去商店");
                Uri gotoStore = new Uri("ms-windows-store://review/?appid=" + CurrentApp.AppId);
                   Windows.System.Launcher.LaunchUriAsync(gotoStore);
                
            }
        }
    }
}

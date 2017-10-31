using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Json;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using HTML.views;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HTML.page
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class shouCangPage : Page
    {
        public shouCangPage()
        {
            this.InitializeComponent();
            this.gridback.Background = ColorTheme.getTheme(); 
        }
        // 1 书签  2 历史纪录
        int typeOfPage;
        ObservableCollection<shuQianMode> shuqian = new ObservableCollection<shuQianMode>();
        ResourceLoader loderr;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            loderr = new ResourceLoader();
            //BitmapImage im = new BitmapImage(new Uri("http://toto176.blog.163.com/favicon.ico"));
            //image.Source = im;

            this.gridViewPaiXu.ItemsSource = shuqian;
            ApplicationDataContainer app = ApplicationData.Current.LocalSettings;
            Dictionary<string, string> dicinfo = e.Parameter as Dictionary<string, string>;

            if (dicinfo != null)
            {
                if (dicinfo.ContainsKey("type"))
                {
                    string type = dicinfo["type"].ToString();
                    if (type.Equals("shuQian"))
                    {
                        typeOfPage = 1;
                        if (app.Values["shuQian"] != null)
                        {
                            //   app.Values.Remove("1");
                            JsonArray json = JsonArray.Parse(app.Values["shuQian"].ToString());
                            for (uint i = 0; i < json.Count; i++)
                            {
                                JsonObject obj = json.GetObjectAt(i);
                                shuQianMode mode = new shuQianMode();
                                mode.name = obj.GetNamedString("name");
                                mode.uri = obj.GetNamedString("uri");
                                Uri u = new Uri(mode.uri);
                                mode.imagUri = "http://" + u.Host + "/favicon.ico";
                                shuqian.Add(mode);
                            }
                            textNumber.Text = json.Count + loderr.GetString("tianShuQian");// "条书签";

                        }
                        else
                        {
                            textNumber.Text = loderr.GetString("noShuQian");//"您还没保存过书签";
                        }
                        textName.Text = loderr.GetString("shuQian2");// "书签";
                    }
                    if (type.Equals("lishi"))
                    {
                        if (app.Values["lishi"] != null)
                        {
                            typeOfPage = 2;
                            //   app.Values.Remove("1");
                            JsonArray json = JsonArray.Parse(app.Values["lishi"].ToString());
                            for (uint i = 0; i < json.Count; i++)
                            {
                                JsonObject obj = json.GetObjectAt(i);
                                shuQianMode mode = new shuQianMode();
                                mode.name = obj.GetNamedString("name");
                                mode.uri = obj.GetNamedString("uri");
                                Uri u = new Uri(mode.uri);
                                mode.imagUri = "http://" + u.Host + "/favicon.ico";
                                shuqian.Add(mode);
                            }
                            textNumber.Text = json.Count + loderr.GetString("tiaoJiLu");//"条历史纪录";

                        }
                        else
                        {
                            textNumber.Text = loderr.GetString("noJiLu");//"无历史记录";
                        }
                        textName.Text = loderr.GetString("liShiJiLu2"); //"历史纪录";
                    }
                    if (type.Equals("xiazai"))

                    {
                        typeOfPage = 3;
                        textNumber.Text = loderr.GetString("wuXiaZai");// "无下载记录";
                        textName.Text = loderr.GetString("xiaZai2");// "下载纪录";
                    }
                    if (type.Equals("shezhi"))

                    {
                        typeOfPage = 4;
                        textNumber.Text = loderr.GetString("liuLanSheZhi");// "浏览器设置";
                        textName.Text = loderr.GetString("sheZhi2"); //"设置";
                        btnForShanChu.Visibility = Visibility.Collapsed;
                        gridViewPaiXu.Visibility = Visibility.Collapsed;
                        SettingView set = new SettingView();
                        gridPane.Children.Add(set);
                    }
                }
            }
          
          
           
        }

        private void backClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
            NotyficationCenter.getNoty().startdelegatmainPageBack(0);
        
        }

        private void gridViewPaiXu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            shuQianMode modeList = gridViewPaiXu.SelectedItem as shuQianMode;
            if (gridViewPaiXu.SelectionMode == ListViewSelectionMode.Single)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("isopen", true);
                dic.Add("shuQianMode", modeList);
                this.Frame.Navigate(typeof(webPage), dic);
                NotyficationCenter.getNoty().startdelegatmainPageBack(0);
            }
          
        }


        private async void bianJiClick(object sender, RoutedEventArgs e)
        {
            //if (gridViewPaiXu.SelectionMode == ListViewSelectionMode.Multiple)
            //{
            //    gridViewPaiXu.SelectionMode = ListViewSelectionMode.Single;
            //}
            //else
            //{
            //    gridViewPaiXu.SelectionMode = ListViewSelectionMode.Multiple;
            //}


            ApplicationDataContainer app = ApplicationData.Current.LocalSettings;
            if (typeOfPage == 1)
            {
                MessageDialog mes = new MessageDialog(loderr.GetString("qingKongShuQian"), loderr.GetString("qingKongTiXing"));
                UICommand OK = new UICommand(loderr.GetString("shanchu"), (IUICommand command) => {
                    app.Values.Remove("shuQian");
                    shuqian.Clear();
                });

                mes.Commands.Add(OK);
                UICommand closs = new UICommand(loderr.GetString("quxiao"), (IUICommand command) => {

                });

                mes.Commands.Add(closs);
                await mes.ShowAsync();
               
            }
            if (typeOfPage == 2)
            {
                MessageDialog mes = new MessageDialog(loderr.GetString("qingkongLishi"), loderr.GetString("qingKongTiXing"));
                UICommand OK = new UICommand(loderr.GetString("shanchu"), (IUICommand command) =>
                {
                    app.Values.Remove("lishi");
                    shuqian.Clear();

                });

                mes.Commands.Add(OK);
                UICommand closs = new UICommand(loderr.GetString("quxiao"), (IUICommand command) =>
                {

                });

                mes.Commands.Add(closs);
                await mes.ShowAsync();
            }

        }
    }
    public class shuQianMode
    {
        public string name { get; set; }
        public string uri { get; set; }
        public string imagUri { get; set; }
    }
}

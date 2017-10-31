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
using Windows.Storage;
using System.Text;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.Data.Json;
using Windows.Web.Http;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HTML.page
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class webPage : Page
    {
        public webPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            mainGrid.Background = ColorTheme.getTheme();
            SystemNavigationManager.GetForCurrentView().BackRequested += phoneBackClick; ;
        }
        /// <summary>
        /// 手机物理按键返回键被触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phoneBackClick(object sender, BackRequestedEventArgs e)
        {
            bool handled = false;
            if (getWebViewForNumber() != null)
            {
                WebView web = getWebViewForNumber();
                if (web.CanGoBack)
                {
                    web.GoBack();
                    handled = true;
                }
            }
            e.Handled = handled;
        }

        static int jsonIndex = 0;
        Dictionary<string, WebView> dic = new Dictionary<string, WebView>();
        int numberIndex = 0;

        DispatcherTimer tier = new DispatcherTimer();
         string UriShouYe1 = "http://hao.360.cn/?src=lm&ls=n683c0f9899";
         string UriShouYe2 = "http://www.2345.com/?k1617176084";
        protected override   void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            Dictionary<string,object> dicinfo =  e.Parameter as Dictionary<string, object>;

            if (dicinfo != null)
            {
                if (dicinfo.ContainsKey("isopen"))
                {
                    shuQianMode mo = dicinfo["shuQianMode"] as shuQianMode;
                    addWevView(mo.uri);
                }
            }
            else
            {
                //开启的新页面
                string key = "jsonUri";
                ApplicationDataContainer app = ApplicationData.Current.LocalSettings;
                string jsonStr = app.Values[key] as string;
                if (jsonStr == null)
                {
                    //则重新发起请求 jsonUri 
                }
                else
                {
                    //则导航到已存在的 jsonUri ,并重新请求
                    //json 格式
                    //{  "uri1": "http://www.baidu.com",  "uri2": "http://www.baidu.com"}
                     
                    //本地此时数据  不要后注释
                   // jsonStr = "{  \"uri1\":\"http://www.baidu.com\",  \"uri2\": \"http://www.baidu.com\"}";
                   
                    JsonObject json = JsonObject.Parse(jsonStr);
                    json = json["data"].GetObject();
                    if (json != null)
                    {
                        string uri1 = json["uri1"].GetString();
                        string uri2 = json["uri2"].GetString();
                        UriShouYe1 = uri1;
                        UriShouYe2 = uri2;
                       // web.Navigate(new Uri(UriShouYe2));
                    }
                  
                }
                //重新获得json并存储到本地
                getJsonUri();
            }
            initShiPei();

            tier.Interval = TimeSpan.FromSeconds(0.25);
            tier.Tick += Tier_Tick;
            //   Frame.Navigate(typeof(page.webPage));
            //Uri uri = new Uri("ms-appx:///Assets/TextFile1.txt");

            //StorageFolder local = ApplicationData.Current.LocalFolder;
            //StorageFile imafFil = await StorageFile.GetFileFromApplicationUriAsync(uri);
            //Stream ss=     await imafFil.OpenStreamForReadAsync();
            //byte[] buff = new byte[ss.Length];
            //ss.Read(buff, 0, buff.Length);
            //string name =  Encoding.UTF8.GetString(buff);
            //web.NavigateToString(name);
            if (dic.Count == 0)
            {
                addWevView(UriShouYe1);
            }
            NotyficationCenter.getNoty().delegatAddWebUrl += async (string _url) =>
            {
                await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () =>
                {
                    // 
                    // UI components can be accessed within this scope.
                    // 
                    addWevView(_url);
                });
                
            };

        }
      async  void getJsonUri()
        {
            jsonIndex++;
            if (jsonIndex == 1)
            {
                //请求URi
                string appName = Package.Current.DisplayName.ToString().Replace(" ", "");
                string uri = "http://www.win10appstore.com/tuixiangzi.php?action=webmainpage&name=" + appName;
                HttpClient client = new HttpClient();
                try
                {
                    HttpResponseMessage respon = await client.GetAsync(new Uri(uri));
                    if (respon.StatusCode == HttpStatusCode.Ok)
                    {
                        string jsonShuChu = respon.Content.ToString();
                        Debug.Write(jsonShuChu);
                        ApplicationDataContainer app = ApplicationData.Current.LocalSettings;
                        app.Values["jsonUri"] = jsonShuChu;
                    }
                    else
                    {
                        Debug.Write("出现错误");
                    }
                }
                catch
                {
                    Debug.Write("出现错误");
                }
            }
        }
        void initShiPei()
        {
            if (Window.Current.Bounds.Width < 480)
            {
                scro.Margin = new Thickness(48, 0, 44, 0);
            }
            else
            {
                scro.Margin = new Thickness(0, 0, 44, 0);
            }

        }
        private void Tier_Tick(object sender, object e)
        {
            scro.ChangeView((stack.Children.Count) * 164.0f, 0, 1, true);
            tier.Stop();
        }

        void addWevView(string UriStr)
        {
            foreach (WebView we in dic.Values)
            {
                we.SetValue(Canvas.ZIndexProperty, 0);
            }
            numberIndex++;
            WebView newWeb =    new WebView(WebViewExecutionMode.SameThread);
            newWeb.Tag = numberIndex;
            newWeb.Width = web.Width;
            newWeb.Height = web.Height;
            newWeb.Margin = web.Margin;

            newWeb.NewWindowRequested += web_NewWindowRequested;
            newWeb.DOMContentLoaded += web_DOMContentLoaded;
            newWeb.NavigationCompleted += web_NavigationCompleted;
            newWeb.FrameContentLoading += web_FrameContentLoading;
            newWeb.FrameNavigationCompleted += web_FrameNavigationCompleted;
            newWeb.FrameDOMContentLoaded += web_FrameDOMContentLoaded;
            newWeb.LoadCompleted += web_LoadCompleted;
            newWeb.NavigationStarting += web_NavigationStarting;
            newWeb.UnsupportedUriSchemeIdentified += web_UnsupportedUriSchemeIdentified;

            newWeb.Navigate(new Uri(UriStr));
            mainGrid.Children.Add(newWeb);
            dic.Add("" + numberIndex, newWeb);
            //  NotyficationCenter.getNoty().startdelegatAddWeb("新加页面");
            addToolTitle("新加页面",numberIndex);
            tier.Start();

        }
        void removView(int _remoIndex)
        {
          WebView wewww =  dic[("" + _remoIndex)]; //("" + numberIndex);
            wewww.Navigate(new Uri("about: blank"));
            wewww.Stop();
            wewww.NavigateToString("");
            dic.Remove("" + _remoIndex);
            mainGrid.Children.Remove(wewww);
            wewww = null;

        }

        void forwordView(int index)
        {
            foreach (WebView we in dic.Values)
            {
                we.SetValue(Canvas.ZIndexProperty,0);
                we.Visibility = Visibility.Collapsed;
            }
            WebView wewww = dic[("" + index)]; //("" + numberIndex);
            wewww.SetValue(Canvas.ZIndexProperty, 100);
            wewww.Visibility = Visibility.Visible;
        }
        private void textBox_TextCompositionEnded(TextBox sender, TextCompositionEndedEventArgs args)
        {
         
        }

        private async void textBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                int index = -1;
                WebView webSelect = null;
                foreach (WebView we in dic.Values)
                {
                    int zindex = int.Parse(we.GetValue(Canvas.ZIndexProperty).ToString());
                    if (index == -1)
                    {
                        index = zindex;
                        webSelect = we;
                    }
                    if (index <= zindex)
                    {
                        index = zindex;
                        webSelect = we;
                    }

                }

                string urlStr = textBox.Text;
                if (textBox.Text.IndexOf("http://") >= 0 || textBox.Text.IndexOf("HTTP://") >= 0|| textBox.Text.IndexOf("https://") >= 0 || textBox.Text.IndexOf("HTTPS://") >= 0)
                {

                }
                else
                {
                    urlStr = "http://" + urlStr;
                }
                if (Uri.IsWellFormedUriString(urlStr, UriKind.Absolute))
                {
                    webSelect.Navigate(new Uri(urlStr));
                }
                else
                {
                    MessageDialog msg = new MessageDialog(urlStr + "不能打开");
                    msg.Title = "网址";
                    await msg.ShowAsync();
                }
            }
        }


        void addToolTitle(string name,int _addIndex)
        {
            btnForPage page = new btnForPage();
            page.Tag = _addIndex;
            page.delegatePageCloss += () => {

                stack.Children.Remove(page);
                removView(_addIndex); 

                 WebView we =   getWebViewForNumber();
                if (we != null)
                {
                    forwordView(int.Parse(we.Tag.ToString()));
                    setSelectPage(int.Parse(we.Tag.ToString()));
                }
                if (dic.Count == 0)
                {
                    addWevView(UriShouYe1);
                }
            };
            page.delegatepageSelect += () => {

                forwordView(_addIndex);
                setSelectPage(_addIndex);
            };
            page.setTitleName(name);
            stack.Children.Add(page);

            forwordView(_addIndex);
            setSelectPage(_addIndex);
          double longWight =  page.Width* stack.Children.Count;
         
        }
        btnForPage getBtnForPageWithTag(int _Tag)
        {
            foreach (btnForPage nowPage in stack.Children)
            {
                if (_Tag == int.Parse(nowPage.Tag.ToString()))
                {
                    return nowPage;
                }
            }
            return null;
        }
        /// <summary>
        /// 获得最高的一个WebView
        /// </summary>
        /// <returns></returns>
        WebView getWebViewForNumber()
        {
            int index = -1;
            WebView webSelect = null;
            foreach (WebView we in dic.Values)
            {
                int zindex = int.Parse(we.GetValue(Canvas.ZIndexProperty).ToString());
                if (index == -1)
                {
                    index = zindex;
                    webSelect = we;
                }
                if (index <= zindex)
                {
                    index = zindex;
                    webSelect = we;
                }

            }
            return webSelect;
        }
        void setSelectPage(int _addIndex)
        {
            foreach (btnForPage nowPage in stack.Children)
            {
                nowPage.setNoSelect();
                if (int.Parse( nowPage.Tag.ToString()) == _addIndex)
                {
                    nowPage.setSelect();
                }
            }

            WebView webNow = getWebViewForNumber();
            textBox.Text = webNow.Source.AbsoluteUri.ToString();
        }
        void setToolTitleName()
        {
         
        
            NotyficationCenter noty = NotyficationCenter.getNoty();
            noty.delegattitleNameForKey += (string _titleName, string _key) => {

                //  addToolTitle(_titlName);
            };
        }

        private void addPageClick(object sender, RoutedEventArgs e)
        {
            addWevView(UriShouYe1);
        }

        private void web_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            //下载不受支持的类型
            Debug.Write("下载不受支持的类型");
        }

        private void web_UnsupportedUriSchemeIdentified(WebView sender, WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            //新增的WebView.UnsupportedUriSchemeldentified事件能够让WebView捕获到不受支持的uri地址，让我们开发者提供处理这些不受支持的URI方案。 
            Debug.WriteLine("新增的WebView.UnsupportedUriSchemeldentified事件能够让WebView捕获到不受支持的uri地址，让我们开发者提供处理这些不受支持的URI方案。 ");
            args.Handled = true;

        }

        private void web_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            //WebView.NewWindowsRequested事件能在WebView捕获脚本请求一个新的浏览器窗口动作。默认情况下当用户点击一个href或者按钮调用Windows.Open时，会启动默认浏览器来打开请求的导航，现在开发者可以自己捕获该动作，自行处理业务逻辑。 
            Debug.WriteLine("WebView.NewWindowsRequested事件能在WebView捕获脚本请求一个新的浏览器窗口动作。默认情况下当用户点击一个href或者按钮调用Windows.Open时，会启动默认浏览器来打开请求的导航，现在开发者可以自己捕获该动作，自行处理业务逻辑。 ");
            args.Handled = true;
            web.Navigate(args.Uri);
            addWevView(args.Uri.AbsoluteUri);
        }

        private void web_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            //在 WebView 已完成当前 HTML 内容的分析时发生。
            Debug.WriteLine("在 WebView 已完成当前 HTML 内容的分析时发生。");
            if (getBtnForPageWithTag(int.Parse(sender.Tag.ToString())) != null)
            {
                getBtnForPageWithTag(int.Parse(sender.Tag.ToString())).setTitleName(sender.DocumentTitle);
            }
        }

        private void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //在 WebView 已完成当前内容的加载或导航已失败时发生。
            Debug.WriteLine("在 WebView 已完成当前内容的加载或导航已失败时发生。");
            if (args.IsSuccess)
            {
                setFile("lishi", sender);
            }
           
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //scroll大小更改后
           
        }

        private void scro_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
          
        }

        private void scro_DirectManipulationCompleted(object sender, object e)
        {
         
        }

        private void webBack(object sender, RoutedEventArgs e)
        {
            if (getWebViewForNumber() != null)
            {
             WebView web =    getWebViewForNumber();
                if (web.CanGoBack)
                {
                    web.GoBack();
                }
            }
        }

        private void webForword(object sender, RoutedEventArgs e)
        {
            if (getWebViewForNumber() != null)
            {
                WebView web = getWebViewForNumber();
                if (web.CanGoForward)
                {
                    web.GoForward();
                }
            }
        }
        private void shuaXinClic(object sender, RoutedEventArgs e)
        {
            if (getWebViewForNumber() != null)
            {
                WebView web = getWebViewForNumber();
                
                 web.Refresh();
               
            }
        }
        private void web_FrameContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            //在 WebView 中的帧开始加载新内容时发生。
            Debug.WriteLine("在 WebView 中的帧开始加载新内容时发生。");
         
        }

        private void web_FrameNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //在 WebView 中的帧完成内容加载时发生。
            Debug.WriteLine("在 WebView 中的帧完成内容加载时发生。");
        }

        private void web_FrameDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            //在 WebView 中的帧完成对其当前 HTML 内容的分析时发生。
            Debug.WriteLine("在 WebView 中的帧完成对其当前 HTML 内容的分析时发生。");
        }

        private void web_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //在顶级导航完成且内容加载到 WebView 控件中时发生，或在加载期间发生错误时发生。
            Debug.WriteLine("在顶级导航完成且内容加载到 WebView 控件中时发生，或在加载期间发生错误时发生。");
            WebView webNow = getWebViewForNumber();
            textBox.Text = webNow.Source.AbsoluteUri.ToString();

           
        }

        private void web_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            //在 WebView 导航至新内容之前发生。
            Debug.WriteLine("在 WebView 导航至新内容之前发生。");
            if (sender.Tag != null)
            {
                if (getBtnForPageWithTag(int.Parse(sender.Tag.ToString())) != null)
                {
                    getBtnForPageWithTag(int.Parse(sender.Tag.ToString())).setTitleName(sender.DocumentTitle);
                }
             
            }
       

        }

        private void mainGrid_LayoutUpdated(object sender, object e)
        {
            initShiPei();
        }

        private void mainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            initShiPei();
        }

        private async void shuQianClick(object sender, RoutedEventArgs e)
        {
            ResourceLoader loader = new ResourceLoader();
            MessageDialog mes = new MessageDialog(loader.GetString("shifouJiarushuqian"), loader.GetString("youqingtixing"));
            UICommand OK = new UICommand(loader.GetString("jiaru"), (IUICommand command) =>
            { 
                WebView webNow = getWebViewForNumber();
                setFile("shuQian", webNow);
            });

            mes.Commands.Add(OK);
            UICommand closs = new UICommand(loader.GetString("zanbujiaru"), (IUICommand command) =>
            {

            });

            mes.Commands.Add(closs);
            await mes.ShowAsync();
         

        }
        void setFile(string value, WebView webNow)
        {
            if (webNow.Source == null) return;
            string uri = webNow.Source.AbsoluteUri.ToString();
            string name = webNow.DocumentTitle;
            ApplicationDataContainer app = ApplicationData.Current.LocalSettings;
            if (app.Values[value] != null)
            {
                JsonArray json = JsonArray.Parse(app.Values[value].ToString());
                JsonObject obj = new JsonObject();
                obj.Add("name", JsonValue.CreateStringValue(name));
                obj.Add("uri", JsonValue.CreateStringValue(uri));
                obj.Add("date", JsonValue.CreateStringValue(DateTime.Now.ToString()));
                json.Add(obj);

                app.Values.Remove(value);
                app.Values.Add(value, json.ToString());
            }
            else
            {
                JsonArray json = new JsonArray();
                JsonObject obj = new JsonObject();
                obj.Add("name", JsonValue.CreateStringValue(name));
                obj.Add("uri", JsonValue.CreateStringValue(uri));
                obj.Add("date", JsonValue.CreateStringValue(DateTime.Now.ToString()));
                json.Add(obj);
                app.Values.Add(value, json.ToString());
            }
        }
 

        private void shuaXinClic(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (getWebViewForNumber() != null)
            {
                WebView web = getWebViewForNumber();

                web.Refresh();

            }
        }
    }
}

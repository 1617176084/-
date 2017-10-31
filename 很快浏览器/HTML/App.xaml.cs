using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Profile;
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
using UmengSDK;
using UmengAnalyticsDemoUWP;
using System.Diagnostics;
using WinPush;

namespace HTML
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        async void OnResuming(object sender, object e)
        {
            await UmengAnalytics.StartTrackAsync(AppConfig.AppKey);
        }

        protected async override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            
            await UmengAnalytics.StartTrackAsync(AppConfig.AppKey);
            Initialize(args);
        }
        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {


            Initialize(e);
            //   Debug.WriteLine("PushNotificationServer  :  " + await PushNotificationServer.notficationUrl());
        }

        async void Initialize(IActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                rootFrame.Navigate(typeof(JYMainPage));
                addNotyfication(e);
            }

            // 确保当前窗口处于活动状态
            Window.Current.Activate();

            await UmengAnalytics.StartTrackAsync(AppConfig.AppKey);
            UmengAnalytics.SetSessionInterval(3);

            await UmengAnalytics.UpdateOnlineParamAsync();

        }
        public void addNotyfication(IActivatedEventArgs e) {
            //确保事件只被初始化了一次，如多次调用则会产生多次回调
            PushNotificationServer winPush = PushNotificationServer.shearServer();
            winPush.onNotificationChannelFinish += WinPush_onNotificationChannelFinish;
            winPush.onNotificationReceived += WinPush_onNotificationReceived; ;
            winPush.onNotuficationSelected += WinPush_onNotuficationSelected;
            winPush.initApplication("87f0227d46b411e6", e as ToastNotificationActivatedEventArgs);
            //应用程序要取消已注册的后台活动
            winPush.UnregisterBackgroundTask("WiFiBackgroundTask");

       
        }
        private void WinPush_onNotificationChannelFinish(PushNotificationServer sender)
        {
            //通知通道注册完成的时候
            //通知通道地址为：  sender.channel.Uri
            Debug.WriteLine("通道获得成功：" + sender.channel.Uri.ToString());
        }

        private void WinPush_onNotificationReceived(PushNotificationServer sender, Windows.Networking.PushNotifications.PushNotificationReceivedEventArgs args)
        {
            //接收到了消息
            // 当自己处理推送通知时请设置Cancel为true
            // args.Cancel = true;

        }

        private void WinPush_onNotuficationSelected(CateporyListModel model, string Argument)
        {
            //当通知消息被点击的时候
            if (model.action == "openHtml")
            {
                NotyficationCenter.getNoty().startdelegatAddWebURl(model.link);
            }
        }
 
         
        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await UmengAnalytics.EndTrackAsync();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
          
           
        }
    }
}

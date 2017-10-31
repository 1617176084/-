using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HTML
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
          
            this.InitializeComponent();
           
          //  StreamReader reader = File.OpenText("/Assets/TextFile1.txt");
       
             webInit();
            getStr();
        }

        async void getStr()
        {
          //  Resources.Values();

            string ss = await ReadTimestamp("./Assets/TextFile1.txt");
        }
        /// <summary>
        /// 获取独立存储文件
        /// </summary>
        private static Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        /// <summary>
        /// 读取文件
        /// </summary>
        public async Task<string> ReadTimestamp(string fileName)
        {
            try
            {
                Windows.Storage.StorageFile sampleFile = await localFolder.GetFileAsync(fileName);
                string contents = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                return contents;
            }
            catch (Exception)
            {
                return "read faild";
            }
        }
        void webInit()
        {
            web.NavigateToString("web 温带");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            stack.Orientation = Orientation.Horizontal;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            stack.Orientation = Orientation.Vertical;
        }
    }
}

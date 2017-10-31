using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML
{
    class NotyficationCenter
    { 
        static NotyficationCenter shader = null;
        public static NotyficationCenter getNoty()
        {
            if (shader == null)
            {
                shader = new NotyficationCenter();
            }
            return shader;
        }

        public delegate void addWebView(string _titleName);
       public  event addWebView delegatAddWeb;
        public void startdelegatAddWeb(string _name)
        {
            delegatAddWeb(_name);
        }

        public delegate void addWebViewUrl(string _titleName);
        public event addWebViewUrl delegatAddWebUrl;
        public void startdelegatAddWebURl(string _Url)
        {
            delegatAddWebUrl(_Url);
        }


        public delegate void titleNameForKey(string _titleName,string _key);
        public event titleNameForKey delegattitleNameForKey;
        public void startdelegattitleNameForKey(string _titleName, string _key)
        {
            delegattitleNameForKey(_titleName, _key);
        }

        public delegate void mainPageBack(int goBackIndex);
        public event mainPageBack delegatmainPageBack;
        public void startdelegatmainPageBack(int goBackIndex)
        {
            delegatmainPageBack(goBackIndex);
        }
    }
}

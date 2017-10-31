using JP.Utils.Data;
using JP.Utils.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace HTML
{
  public    class ColorTheme
    {
        /// <summary>
        /// 设置主题色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool settingTheme(Color color) {
          return  LocalSettingHelper.AddValue("colorTheme", ColorConverter.RGBToHex(int.Parse(color.R.ToString()), int.Parse(color.G.ToString()), int.Parse(color.B.ToString())));
        }

        public static bool settingTheme(string color)
        {
            return LocalSettingHelper.AddValue("colorTheme", color);
        }
        /// <summary>
        /// 读取主题色
        /// </summary>
        /// <returns></returns>
        public static SolidColorBrush getTheme() {
            string colorName = LocalSettingHelper.GetValue("colorTheme");
           return  new SolidColorBrush(ColorConverter.HexToColor(colorName).Value);
        }
    }
}

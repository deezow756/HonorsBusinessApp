using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using BusinessApp.Utilities;

namespace BusinessApp.Themes
{
    public enum ThemeType { Light, Dark}
    public class ThemeHelper
    {
        public static ThemeType CurrentTheme;
        public static void ChangeTheme()
        {
            var curTheme = AppInfo.RequestedTheme;
            switch (curTheme)
            {
                case AppTheme.Unspecified:
                    Application.Current.Resources = new LightTheme();
                    CurrentTheme = ThemeType.Light;
                    break;
                case AppTheme.Light:
                    Application.Current.Resources = new LightTheme();
                    CurrentTheme = ThemeType.Light;
                    break;
                case AppTheme.Dark:
                    Application.Current.Resources = new DarkTheme();
                    CurrentTheme = ThemeType.Dark;
                    break;
                default:
                    break;
            }
        }

        public static void ChangeTheme(ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.Light:
                    Application.Current.Resources = new LightTheme();
                    CurrentTheme = ThemeType.Light;
                    break;
                case ThemeType.Dark:
                    Application.Current.Resources = new DarkTheme();
                    CurrentTheme = ThemeType.Dark;
                    break;
                default:
                    break;
            }
        }
    }
}

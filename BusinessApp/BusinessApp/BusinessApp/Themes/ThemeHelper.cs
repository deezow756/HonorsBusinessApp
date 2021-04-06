using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using BusinessApp.Utilities;
using BusinessApp.Resources.Fonts;

namespace BusinessApp.Themes
{
    public enum ThemeType {Light, Dark}
    public class ThemeHelper
    {
        public static EventHandler ThemeChanged;
        public static void OnThemeChanged(EventArgs e)
        {
            EventHandler handler = ThemeChanged;
            if(handler != null)
            {
                handler(null, e);
            }
        }

        public static void RegisterToOnThemeChanged()
        {

        }

        private static ThemeType currentTheme;
        public static ThemeType CurrentTheme
        {
            get { return currentTheme; }
            set
            {
                currentTheme = value;
                OnThemeChanged(null);
            }
        }
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
            FontHelper.ChangeFont(FileManager.LoadSettings().Font);
        }
        public static void ChangeTheme(FontType font)
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
            FontHelper.ChangeFont(font);
        }

        public static void ChangeTheme(ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.Light:
                    Application.Current.Resources = new LightTheme();
                    CurrentTheme = ThemeType.Light;
                    MessagingCenter.Send<object, ThemeType>(Application.Current, "ChangeTheme", ThemeType.Light);
                    break;
                case ThemeType.Dark:
                    Application.Current.Resources = new DarkTheme();
                    CurrentTheme = ThemeType.Dark;
                    break;
                default:
                    break;
            }
            FontHelper.ChangeFont(FileManager.LoadSettings().Font);
        }

        public static void ChangeTheme(ThemeType theme, FontType font)
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

            FontHelper.ChangeFont(font);
        }
    }
}

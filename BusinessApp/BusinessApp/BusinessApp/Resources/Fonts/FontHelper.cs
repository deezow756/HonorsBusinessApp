using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Resources.Fonts
{
    public enum FontType { Default, Tech, Amperzand }
    public class FontHelper
    {
        private static Dictionary<FontType, string> Fonts = new Dictionary<FontType, string>() { 
            { FontType.Default, "" }, { FontType.Amperzand, "Amperzand" }, { FontType.Tech, "Tech" } };

        public static void ChangeFont(FontType font)
        {
            Application.Current.Resources["CurrentFont"] = Fonts[font];
        }

        public static List<FontType> GetFonts()
        {
            List<FontType> fonts = new List<FontType>();

            foreach (KeyValuePair<FontType, string> entry in Fonts)
            {
                fonts.Add(entry.Key);
            }

            return fonts;
        }

        public static string GetFont(FontType font)
        {
            return Fonts[font];
        }

        public static FontType GetFontType(string font)
        {
            foreach (KeyValuePair<FontType, string> entry in Fonts)
            {
                if(entry.Value == font)
                {
                    return entry.Key;
                }
            }
            return FontType.Default;
        }
    }
}

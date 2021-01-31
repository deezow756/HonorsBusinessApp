using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Resources.Fonts
{
    public enum FontType { Default, Samantha, Rosallia }
    public class FontHelper
    {
        private static Dictionary<FontType, string> Fonts = new Dictionary<FontType, string>() { { FontType.Default, "Default" }, { FontType.Rosallia, "Rosallia" }, { FontType.Samantha, "Samantha" } };

        public static void ChangeFont(FontType font)
        {
            Application.Current.Resources["CurrentFont"] = Fonts[font];
        }
    }
}

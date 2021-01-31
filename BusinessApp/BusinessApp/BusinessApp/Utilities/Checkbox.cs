using BusinessApp.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Utilities
{
    public class Checkbox
    {
        internal class ImageSource
        {
            private static double size = 30;
            public static FontImageSource GetChecked()
            {
                if(ThemeHelper.CurrentTheme == ThemeType.Dark)
                {
                    return new FontImageSource() { Glyph = "\uf14a", Size = size, FontFamily = "FAS", Color = Color.White };
                }
                else
                {
                    return new FontImageSource() { Glyph = "\uf14a", Size = size, FontFamily = "FAS", Color = Color.Black };
                }
            }
            public static FontImageSource GetUnchecked()
            {
                if (ThemeHelper.CurrentTheme == ThemeType.Dark)
                {
                    return new FontImageSource() { Glyph = "\uf14a", Size = size, FontFamily = "FAR", Color = Color.White };
                }
                else
                {
                    return new FontImageSource() { Glyph = "\uf14a", Size = size, FontFamily = "FAR", Color = Color.Black };
                }
            }
        }
    }
}

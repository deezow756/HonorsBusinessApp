using System;
using System.Collections.Generic;
using System.Text;
using BusinessApp.Themes;
using BusinessApp.Resources.Fonts;

namespace BusinessApp.Models
{
    public class Settings
    {
        public bool AutoTheme { get; set; }
        public ThemeType Theme { get; set; }
        public FontType Font { get; set; }
    }
}

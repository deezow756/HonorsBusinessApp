using BusinessApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BusinessApp.Utilities
{
    public class FileManager
    {
        private static string loginFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "login.json");
        public static Settings LoadSettings()
        {
            if(File.Exists(loginFilePath))
            {
                Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(loginFilePath));
                return settings;
            }
            else
            {
                Settings settings = new Settings() { AutoTheme = true, Theme = Themes.ThemeType.Light, Font = Resources.Fonts.FontType.Default };
                File.WriteAllText(loginFilePath, JsonConvert.SerializeObject(settings, Formatting.None));
                return settings;
            }
        }

        public static void SaveSettings(Settings settings)
        {
            File.WriteAllText(loginFilePath, JsonConvert.SerializeObject(settings, Formatting.None));
        }
    }
}

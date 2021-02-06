using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Themes;
using BusinessApp.Resources.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusinessApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        bool setup = false;
        Settings settings;
        List<FontType> fontTypes;
        public SettingsView()
        {
            settings = FileManager.LoadSettings();
            InitializeComponent();
            
            if(settings.AutoTheme)
            {
                manualThemeLayout.IsVisible = false;
                btnAutoTheme.Style = Application.Current.Resources["ToggleOn"] as Style;
            }
            else
            {
                manualThemeLayout.IsVisible = true;
                btnAutoTheme.Style = Application.Current.Resources["ToggleOff"] as Style;

                if(settings.Theme == Themes.ThemeType.Light)
                {
                    btnLightTheme.Style = Application.Current.Resources["LightThemeOn"] as Style;
                    btnDarkTheme.Style = Application.Current.Resources["DarkThemeOff"] as Style;
                }
                else
                {
                    btnLightTheme.Style = Application.Current.Resources["LightThemeOff"] as Style;
                    btnDarkTheme.Style = Application.Current.Resources["DarkThemeOn"] as Style;
                }
            }

            SetFontsList();

            setup = true;
        }       
        
        private void SetFontsList()
        {
            List<Resources.Fonts.Font> fonts = new List<Resources.Fonts.Font>();

            fontTypes = FontHelper.GetFonts();

            for (int i = 0; i < fontTypes.Count; i++)
            {
                string font = FontHelper.GetFont(fontTypes[i]);
                if (font == "")
                    fonts.Add(new Resources.Fonts.Font() { Name = "Default", FontFamily = "" });
                else
                    fonts.Add(new Resources.Fonts.Font() { Name = font, FontFamily = font });
            }

            lstFonts.ItemsSource = null;
            lstFonts.ItemsSource = fonts;
        }

        private void btnAutoTheme_Clicked(object sender, EventArgs e)
        {
            if (settings.AutoTheme)
            {
                settings.AutoTheme = false;
                manualThemeLayout.IsVisible = true;
                btnAutoTheme.Style = Application.Current.Resources["ToggleOff"] as Style;

                ThemeHelper.ChangeTheme(settings.Theme, settings.Font);
                if (settings.Theme == Themes.ThemeType.Light)
                {
                    btnLightTheme.Style = Application.Current.Resources["LightThemeOn"] as Style;
                    btnDarkTheme.Style = Application.Current.Resources["DarkThemeOff"] as Style;
                }
                else
                {
                    btnLightTheme.Style = Application.Current.Resources["LightThemeOff"] as Style;
                    btnDarkTheme.Style = Application.Current.Resources["DarkThemeOn"] as Style;
                }
            }
            else
            {
                settings.AutoTheme = true;
                manualThemeLayout.IsVisible = false;
                btnAutoTheme.Style = Application.Current.Resources["ToggleOn"] as Style;
                ThemeHelper.ChangeTheme(settings.Font);             
            }

            SetFontsList();
        }

        private void btnLightTheme_Clicked(object sender, EventArgs e)
        {
            settings.Theme = ThemeType.Light;
            ThemeHelper.ChangeTheme(settings.Theme, settings.Font);

            btnLightTheme.Style = Application.Current.Resources["LightThemeOn"] as Style;
            btnDarkTheme.Style = Application.Current.Resources["DarkThemeOff"] as Style;

            SetFontsList();
        }

        private void btnDarkTheme_Clicked(object sender, EventArgs e)
        {
            settings.Theme = ThemeType.Dark;
            ThemeHelper.ChangeTheme(settings.Theme, settings.Font);

            btnLightTheme.Style = Application.Current.Resources["LightThemeOff"] as Style;
            btnDarkTheme.Style = Application.Current.Resources["DarkThemeOn"] as Style;

            SetFontsList();
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            btnBack_Clicked(null, null);
            return true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            FileManager.SaveSettings(settings);
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            lstFonts.SelectedItem = null;
            if (!setup)
                return;

            var vc = (ViewCell)sender;

            FontHelper.ChangeFont(FontHelper.GetFontType(vc.ClassId));
            settings.Font = FontHelper.GetFontType(vc.ClassId);
        }
    }
}
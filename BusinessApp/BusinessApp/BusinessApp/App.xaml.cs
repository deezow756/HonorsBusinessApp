using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using BusinessApp.Themes;
using BusinessApp.Utilities;
using BusinessApp.Models;
using BusinessApp.Resources.Fonts;

namespace BusinessApp
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            InitializeComponent();
            Settings settings = FileManager.LoadSettings();
            if (settings.AutoTheme)
            {
                ThemeHelper.ChangeTheme();
            }
            else
            {
                ThemeHelper.ChangeTheme(settings.Theme);
            }
            FontHelper.ChangeFont(settings.Font);
            MainPage = new NavigationPage(new LoginView());
            StartListening();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            
        }

        public void StartListening()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public void StopListening()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                if (MainPage.Navigation.NavigationStack[MainPage.Navigation.NavigationStack.Count - 1].GetType() != typeof(LoginView))
                {
                    await MainPage.Navigation.PushAsync(new NoInternetView());
                }
                else
                {
                    MessagingCenter.Send<App>(this, "internetOff");
                }
            }
            else if (e.NetworkAccess == NetworkAccess.Internet)
            {
                if (MainPage.Navigation.NavigationStack[MainPage.Navigation.NavigationStack.Count - 1].GetType() == typeof(NoInternetView))
                {
                    await MainPage.Navigation.PopAsync();
                }
                else if(MainPage.Navigation.NavigationStack[MainPage.Navigation.NavigationStack.Count - 1].GetType() == typeof(LoginView))
                {
                    MessagingCenter.Send<App>(this, "internetOn");
                }
            }
        }
    }
}
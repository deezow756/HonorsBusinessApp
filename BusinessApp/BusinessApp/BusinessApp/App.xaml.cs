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

namespace BusinessApp
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            DependencyService.Register<IUpdateRenderers>();
            InitializeComponent();
            ThemeHelper.ChangeTheme();
            MainPage = new NavigationPage(new LoginView());            
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
    }
}
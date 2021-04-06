using BusinessApp.Controllers;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace BusinessApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetView : ContentPage
    {
        NoInternetController controller;
        public NoInternetView()
        {
            controller = new NoInternetController();
            InitializeComponent();
            LoadingPopup();
        }

        protected override bool OnBackButtonPressed()
        {
            btnReturn_Clicked(null, null);
            return true;
        }

        private async void btnReturn_Clicked(object sender, EventArgs e)
        {
            var result = await controller.AreYouSure();
            if (result)
            {
                await Navigation.PopToRootAsync();
            }           
        }

        private void LoadingPopup()
        {
            LoadingPopup page = new LoadingPopup();
            frameArcContentView.Content = page;
        }
    }
}
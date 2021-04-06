using BusinessApp.Controllers;
using BusinessApp.Models;
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
    public partial class LoginView : ContentPage
    {
        LoginController controller;
        public LoginView()
        {
            controller = new LoginController();
            InitializeComponent();

            MessagingCenter.Subscribe<App>(this, "internetOn", sender =>
            {
                HideNoInternet();
            });

            MessagingCenter.Subscribe<App>(this, "internetOff", sender =>
            {
                ShowNoInternet();
            });
        }

        bool showPassword = false;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                ShowNoInternet();
            }
            else
            {
                HideNoInternet();
            }
            showPassword = false;
            txtPassword.IsPassword = true;
            showPass.Style = Application.Current.Resources["RadioUnchecked"] as Style;
        }

        private void btnRegister_Clicked(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            Navigation.PushAsync(new RegisterUserView());
        }

        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            LoadingPopup();

            string tempEmail = txtEmail.Text;          
            string tempPassword = txtPassword.Text;

            if(!string.IsNullOrWhiteSpace(tempEmail))
            {
                tempEmail = tempEmail.Trim();
            }
            if (!string.IsNullOrWhiteSpace(tempPassword))
            {
                tempPassword = tempPassword.Trim();
            }

            var result = controller.CheckLoginValues(tempEmail, tempPassword);

            if (!result)
            {
                ClosePopup();
                return;
            }

            User result2 = await controller.LoginAsync(tempEmail, tempPassword);

            if (result2 == null)
            {
                ClosePopup();
                return;
            }
            else
            {
                ClosePopup();
                txtEmail.Text = "";
                txtPassword.Text = "";
                await Application.Current.MainPage.Navigation.PushAsync(new MenuView(result2));
            }
        }

        private void LoadingPopup()
        {
            arcFrame.IsVisible = true; //visible the frame
            //Scroll.ScrollToAsync(arcFrame, ScrollToPosition.Center, true); //scrolls so that the frame is at the center of the list
            MainContent.Opacity = 0.3; //set the main grid opacity to low
            MainContent.InputTransparent = true; //set the main grid not touchable
            LoadingPopup page = new LoadingPopup();
            frameArcContentView.Content = page;
        }

        private void ClosePopup()
        {
            arcFrame.IsVisible = false; //hide the frame
            MainContent.Opacity = 1; //make back the opacity of main grid
            MainContent.InputTransparent = false; //make main grid touchable
        }

        private void showPass_Clicked(object sender, EventArgs e)
        {
            if (showPassword)
            {
                showPassword = false;
                txtPassword.IsPassword = true;
                showPass.Style = Application.Current.Resources["RadioUnchecked"] as Style;
            }
            else
            {
                showPassword = true;
                txtPassword.IsPassword = false;
                showPass.Style = Application.Current.Resources["RadioChecked"] as Style;
            }
        }

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }

        private async void ShowNoInternet()
        {
            noInternetLayout.FadeTo(1, 500);
        }

        private async void HideNoInternet()
        {
            noInternetLayout.FadeTo(0, 500);
        }
    }
}
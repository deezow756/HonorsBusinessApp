using BusinessApp.Controllers;
using BusinessApp.Utilities;
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
    public partial class LoginView : ContentPage
    {
        LoginController controller;
        public LoginView()
        {
            controller = new LoginController();
            InitializeComponent();       
        }

        bool showPassword = false;

        protected override void OnAppearing()
        {
            base.OnAppearing();
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

            var result = controller.CheckLoginValues(tempEmail, tempPassword);


            if (!result)
            {
                ClosePopup();
                return;
            }

            result = await controller.LoginAsync(tempEmail, tempPassword);

            if (!result)
            {
                ClosePopup();
                return;
            }
            else
            {
                txtEmail.Text = "";
                txtPassword.Text = "";
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
                showPass.Style = Application.Current.Resources["RadioUnchecked"]as Style;
            }
            else
            {
                showPassword = true;
                txtPassword.IsPassword = false;
                showPass.Style = Application.Current.Resources["RadioChecked"] as Style;
            }
        }
    }
}
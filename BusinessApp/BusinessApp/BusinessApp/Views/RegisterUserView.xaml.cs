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

namespace BusinessApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserView : ContentPage
    {
        RegisterUserController controller;
        private bool Matched = false;
        private bool companySwitch = false;

        public RegisterUserView()
        {
            controller = new RegisterUserController();
            InitializeComponent();
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            string password = txtPassword.Text;
            if (!string.IsNullOrEmpty(password))
            {
                if (password.Length >= 6)
                {
                    txtPasswordError.TextColor = Color.Green;
                    txtPasswordError.Text = "Password Accepted";
                    txtPasswordConfirm.IsEnabled = true;
                    txtPasswordConfirm.Text = "";
                    txtConPasswordError.IsVisible = false;
                }
                else
                {
                    txtPasswordError.TextColor = Color.Black;
                    txtPasswordError.Text = "Password Must Be At Least 6 Charaters Long";
                    txtPasswordConfirm.Text = "";
                    txtPasswordConfirm.IsEnabled = false;
                    txtConPasswordError.IsVisible = false;
                }
            }
        }

        private void txtPasswordConfirm_TextChanged(object sender, TextChangedEventArgs e)
        {
            string conPassword = txtPasswordConfirm.Text;
            if (!string.IsNullOrEmpty(conPassword))
            {
                if (conPassword.Length > 3)
                {
                    if (conPassword != txtPassword.Text)
                    {
                        txtConPasswordError.IsVisible = true;
                        Matched = false;
                    }
                    else
                    {
                        txtConPasswordError.IsVisible = false;
                        Matched = true;
                    }
                }
                else
                {
                    if (Matched)
                    {
                        Matched = false;
                        txtConPasswordError.IsVisible = true;
                    }
                }
            }
            else
            {
                Matched = false;
            }
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            FirstLoaderPopup();
            string tempFirstName = txtFirstName.Text;
            string tempSurname = txtSurname.Text;
            string tempEmail = txtEmail.Text;

            if (!controller.CheckRegistrationDetails(this, tempFirstName, tempSurname, Matched, tempEmail))
            {
                ClosePopup();
                return;
            }

            tempEmail = tempEmail.ToLower();

            string tempPassword = txtPassword.Text;            

            if(companySwitch)
            {
                ClosePopup();
                await Navigation.PushAsync(new RegisterCompanyView(new User(tempFirstName, tempSurname, tempEmail, tempPassword)));
            }
            else
            {
                if (!controller.Register(this, tempFirstName, tempSurname, tempEmail, tempPassword))
                {
                    ClosePopup();
                    return;
                }
                else
                {
                    ClosePopup();
                    btnBack_Clicked(null, null);
                }
            }
        }
        private void FirstLoaderPopup()
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

        protected override bool OnBackButtonPressed()
        {
            btnBack_Clicked(null, null);
            return true;            
        }
        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void switchCompany_Clicked(object sender, EventArgs e)
        {
            if (companySwitch)
            {
                switchCompany.Style = Application.Current.Resources["ToggleOff"] as Style;
                btnRegister.Text = "Finish";
                companySwitch = false;
            }
            else
            {
                switchCompany.Style = Application.Current.Resources["ToggleOn"] as Style;
                btnRegister.Text = "Next";
                companySwitch = true;
            }
        }
    }
}
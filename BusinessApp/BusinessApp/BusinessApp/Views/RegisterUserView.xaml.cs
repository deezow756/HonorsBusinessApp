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
            if (!string.IsNullOrWhiteSpace(password))
            {
                password = password.Trim();
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
            if (!string.IsNullOrWhiteSpace(conPassword))
            {
                conPassword = conPassword.Trim();
                if (conPassword.Length > 3)
                {
                    if (conPassword != txtPassword.Text.Trim())
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

            if (!string.IsNullOrWhiteSpace(tempFirstName))
            {
                tempFirstName = tempFirstName.Trim();
                string temp1 = "";
                for (int i = 0; i < tempFirstName.Length; i++)
                {
                    if (i == 0)
                        temp1 += tempFirstName[i].ToString().ToUpper();
                    else
                        temp1 += tempFirstName[i].ToString();
                }
                tempFirstName = temp1;
            }
            if (!string.IsNullOrWhiteSpace(tempSurname))
            {
                tempSurname = tempSurname.Trim();
                string temp2 = "";
                for (int i = 0; i < tempSurname.Length; i++)
                {
                    if (i == 0)
                        temp2 += tempSurname[i].ToString().ToUpper();
                    else
                        temp2 += tempSurname[i].ToString();
                }
                tempSurname = temp2;
            }
            if(!string.IsNullOrWhiteSpace(tempEmail))
            {
                tempEmail = tempEmail.Trim().ToLower();
            }

            var result = await controller.CheckRegistrationDetails(tempFirstName, tempSurname, Matched, tempEmail);
            if (!result)
            {
                ClosePopup();
                return;
            }

            string tempPassword = txtPassword.Text;            

            if(companySwitch)
            {
                ClosePopup();
                await Navigation.PushAsync(new RegisterCompanyView(new User(tempFirstName, tempSurname, tempEmail, Security.EncryptPassword(tempPassword))));
            }
            else
            {
                result = await controller.Register(tempFirstName, tempSurname, tempEmail, tempPassword);
                if (!result)
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

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }
    }
}
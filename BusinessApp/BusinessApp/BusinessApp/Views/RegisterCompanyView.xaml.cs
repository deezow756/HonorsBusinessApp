using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Controllers;
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
    public partial class RegisterCompanyView : ContentPage
    {
        RegisterCompanyController controller;
        private User user;

        private List<Role> roles;

        public RegisterCompanyView(User _user)
        {
            controller = new RegisterCompanyController();
            user = _user;
            InitializeComponent();
            roles = new List<Role>();
        }

        private async void btnGenerate_Clicked(object sender, EventArgs e)
        {
            if (roles.Count > 0)
                btnFinish.IsEnabled = true;

            txtCompanyId.Text = controller.GenerateCompanyID();
        }        

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnFinish_Clicked(object sender, EventArgs e)
        {
            FirstLoaderPopup();

            string temp = txtCompanyName.Text;

            if (controller.Register(user, temp, roles))
            {
                ClosePopup();
                return;
            }
            else
            {
                ClosePopup();
                await DisplayAlert("Success", "Successfully Created Account And Company", "Ok");
                await Navigation.PopToRootAsync();
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

        private void btnAddRole_Clicked(object sender, EventArgs e)
        {
            string temp = txtCompanyId.Text;

            if (!string.IsNullOrEmpty(temp))
            {
                btnFinish.IsEnabled = true;
            }

            roles.Add(new Role() { Name = txtRole.Text });

            lstRoles.ItemsSource = null;
            lstRoles.ItemsSource = roles;

            txtRole.Text = "";
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            var vc = (ViewCell)sender;

            var result = await DisplayAlert("Warning", "Are You Sure You Want To Delete This Role?", "Yes", "No");

            if (result)
            {
                roles.Remove(roles.Find(a => a.Name == vc.ClassId));

                lstRoles.ItemsSource = null;
                if (roles.Count > 0)
                {
                    lstRoles.ItemsSource = roles;
                }
                else
                {
                    btnFinish.IsEnabled = false;
                }
            }
        }

        private void txtRole_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = txtRole.Text;
            if (string.IsNullOrEmpty(temp))
            {
                btnAddRole.IsEnabled = false;
            }
            else
            {
                btnAddRole.IsEnabled = true;
            }
        }
    }
}
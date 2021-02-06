using BusinessApp.Models;
using BusinessApp.Controllers;
using BusinessApp.Views;
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
    public partial class MenuView : ContentPage
    {
        private bool setup = false;
        private User user;
        private MenuController controller;

        public MenuView(User user)
        {
            this.user = user;
            controller = new MenuController();
            InitializeComponent();
            Setup();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (setup)
            {
                Refresh();
            }
            else
            {
                setup = true;
            }
        }

        private async void Refresh()
        {
            user = await controller.GetUser(user.Email);
            Setup();
        }

        private void Setup()
        {
            var result = controller.CheckCompanyIDs(user.CompanyIDs);
            if(result)
            {
                MenuSetup();
            }
            else
            {
                NoCompanySetup();   
            }

        }

        private async void MenuSetup()
        {
            companyLayout.IsVisible = true;
            noCompanyLayout.IsVisible = false;

            await controller.MenuSetup(companyPicker, user.CompanyIDs);

            companyPicker.SelectedIndex = 0;
        }

        private void NoCompanySetup()
        {
            companyLayout.IsVisible = false;
            noCompanyLayout.IsVisible = true;
        }

        private void companyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            if (picker.SelectedIndex < 0)
                return;

            var result = controller.CheckCompanyApproved(user, picker.Items[picker.SelectedIndex]);
            if(result)
            {
                menuButtonsLayout.IsVisible = true;
                companyNotApprovedLayout.IsVisible = false;

                if(controller.CheckCompanyAccess(user, picker.Items[picker.SelectedIndex]) > 1)
                {
                    btnManager.IsVisible = true;
                    btnStatistics.IsVisible = true;
                }
                else
                {
                    btnManager.IsVisible = false;
                    btnStatistics.IsVisible = false;
                }
            }
            else
            {
                menuButtonsLayout.IsVisible = false;
                companyNotApprovedLayout.IsVisible = true;
            }
        }

        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            var result = await controller.ConnectWithCompany(user, txtCompanyID.Text.Trim());
        }

        private async void btnSettings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsView());
        }        

        private async void btnProfile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView(user));
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

        #region MenuButtons        

        private void btnOrders_Clicked(object sender, EventArgs e)
        {

        }

        private void btnAddOrder_Clicked(object sender, EventArgs e)
        {

        }

        private void btnStocks_Clicked(object sender, EventArgs e)
        {

        }

        private void btnManager_Clicked(object sender, EventArgs e)
        {

        }

        private void btnStatistics_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnCreateCompanyAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterCompanyView(user));
        }
        #endregion
        
    }
}
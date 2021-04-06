using BusinessApp.Models;
using BusinessApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BusinessApp.Utilities;

namespace BusinessApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockInfoView : ContentPage
    {
        StockInfoController controller;
        User user;
        Company company;
        StockItem item;
        private bool pageOpen = true;

        int oldQuantity;
        int quantityAmountChanged = 0;

        public StockInfoView(User user, Company company, StockItem item)
        {
            this.user = user;
            this.company = company;
            this.item = item;
            controller = new StockInfoController();
            InitializeComponent();
        }

        private async void Setup()
        {
            FirstLoaderPopup();
            var result = await controller.StockItemCheck(item);
            if(!result)
            {
                ClosePopup();
                Dialog.Show("Warning", "Sorry this Stock Item Is Currently In Use\nTry Again Later", "Ok");
                Navigation.PopAsync();
            }

            await controller.SetActive(item, true);

            txtTitle.Text = item.Name;
            txtQuantity.Text = item.Quantity.ToString();
            oldQuantity = item.Quantity;
            txtPrice.Text = item.Price.ToString();
            txtDescription.Text = item.Description;

            if(item.Quantity == 0)
            { btnMinus.IsEnabled = false; }

            ClosePopup();

            CheckAvailability();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Setup();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            pageOpen = false;
            await controller.SetActive(item, false);
        }
        protected override bool OnBackButtonPressed()
        {
            btnBack_Clicked(null, null);
            return true;
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async Task CheckAvailability()
        {
            Task.Run(Check);

            Device.StartTimer(TimeSpan.FromSeconds(15), () =>
            {
                if (!pageOpen)
                    return false;
                Task.Run(Check);
                return true;
            });

            if (!pageOpen)
                return;
        }

        private async Task Check()
        {
            StockItem tempItem = await controller.GetStockItem(company.CompanyNumber, item.StockNumber);

            if (tempItem == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Dialog.Show("Warning", "This Item Has Been Deleted", "Ok");
                    await Navigation.PopAsync();
                });
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

        private void btnMinus_Clicked(object sender, EventArgs e)
        {
            btnSave.IsVisible = true;
            int quantity = 0;
            try { quantity = int.Parse(txtQuantity.Text); }
            catch (Exception ex) { DisplayAlert("Error: ", ex.Message, "Dismiss"); }

            quantity--;
            quantityAmountChanged--;

            if (quantity == 0) { btnMinus.IsEnabled = false; }
            txtQuantity.Text = quantity.ToString();

            if (quantity == oldQuantity)
            { btnSave.IsVisible = false; }
        }

        private void btnPlus_Clicked(object sender, EventArgs e)
        {
            btnSave.IsVisible = true;
            int quantity = 0;
            try { quantity = int.Parse(txtQuantity.Text); }
            catch (Exception ex) { DisplayAlert("Error: ", ex.Message, "Dismiss"); }

            quantity++;
            quantityAmountChanged++;

            txtQuantity.Text = quantity.ToString();
            btnMinus.IsEnabled = true;

            if (quantity == oldQuantity)
            { btnSave.IsVisible = false; }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            FirstLoaderPopup();

            if (quantityAmountChanged > 0 || quantityAmountChanged < 0)
            {
                await controller.SaveChanges(user, company, item, quantityAmountChanged);
                ClosePopup();
                await Navigation.PopAsync();
            }
            else
            {
                ClosePopup();
                Dialog.Show("Warning", "No Changes Have Been Made", "Ok");
            }

            
        }

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }
    }
}
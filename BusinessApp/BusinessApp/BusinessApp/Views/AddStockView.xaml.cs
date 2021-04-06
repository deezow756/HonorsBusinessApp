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
    public enum AddStockMode { Item, Category}
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStockView : ContentPage
    {
        AddStockController controller;
        User user;
        Company company;
        string category;
        AddStockMode mode;

        public AddStockView(User user, Company company, string category)
        {
            this.user = user;
            this.company = company;
            this.category = category;
            controller = new AddStockController();
            InitializeComponent();
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            FirstLoaderPopup();
            if(mode == AddStockMode.Item)
            {
                SaveItem();
            }
            else
            {
                SaveCategory();
            }
        }

        private async void SaveItem()
        {
            string name = itemNameEntry.Text;
            string price = itemPriceEntry.Text;
            string cost = itemCostEntry.Text;
            string quantity = itemQuantityEntry.Text;
            string description = itemDescriptionEntry.Text;

            if(!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }
            if (!string.IsNullOrWhiteSpace(price))
            {
                price = price.Trim();
            }
            if (!string.IsNullOrWhiteSpace(cost))
            {
                cost = cost.Trim();
            }
            if (!string.IsNullOrWhiteSpace(quantity))
            {
                quantity = quantity.Trim();
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                description = description.Trim();
            }

            var result = controller.CheckDetails(name, price, cost, quantity, description);
            if(!result)
            {
                ClosePopup();
                return;
            }

            await controller.AddStockItem(user, company, name, price, cost, quantity, description, category);

            ClosePopup();
            await Navigation.PopAsync();
        }

        private async void SaveCategory()
        {
            string name = catNameEntry.Text;

            if(!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            var result = controller.CheckDetails(name);
            if (!result)
            {
                ClosePopup();
                return;
            }

            await controller.AddStockCategory(user, company, name, category);

            ClosePopup();
            await Navigation.PopAsync();
        }

        private void modePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(modePicker.SelectedIndex == 0)
            {
                mode = AddStockMode.Item;
                itemLayout.IsVisible = true;
                categoryLayout.IsVisible = false;
            }
            else
            {
                mode = AddStockMode.Category;
                itemLayout.IsVisible = false;
                categoryLayout.IsVisible = true;
            }

            btnSave.IsVisible = true;
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

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }
    }
}
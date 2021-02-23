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
    public partial class AddOrderView : ContentPage
    {
        AddOrderController controller;
        User user;
        Company company;

        List<ItemListEntry> items = new List<ItemListEntry>();

        bool active = false;
        public AddOrderView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new AddOrderController();
            InitializeComponent();
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

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(active)
                return;
            Entry entry = (Entry)sender;
            ItemListEntry item = items.Find(a => a.ItemNumber == entry.ClassId);           
            if(item != null)
            {
                active = true;
                bool result = true;
                bool doCheck = true;
                if(item.Type == ItemType.Basket)
                {
                    if (string.IsNullOrWhiteSpace(entry.Text))
                    {
                        doCheck = false;
                        item.Quantity = 0;
                    }
                    else
                    {
                        result = controller.CheckQuantity(entry.Text);
                    }
                }
                else if (item.Type == ItemType.Labour)
                {
                    if (string.IsNullOrWhiteSpace(entry.Text))
                    {
                        doCheck = false;
                        item.Quantity = 0;
                    }
                    else
                    {
                        result = controller.CheckPerHour(entry.Text);
                    }
                }
                if(!result)
                {
                    return;
                }


                if (doCheck)
                {
                    if (item.Type == ItemType.Basket)
                    {
                        int quan = int.Parse(entry.Text);
                        if (quan < 0)
                        {
                            await Dialog.Show("Warning", "Quantity Cannot Be A Minus Number", "Ok");
                            entry.Text = "";
                        }
                        else
                        {
                            StockItem stock = await controller.GetStockItem(company, item.ItemNumber);
                            if (quan <= stock.Quantity)
                            {
                                item.Quantity = quan;
                            }
                            else
                            {
                                await Dialog.Show("Warning", "Not Enough Stock For This Quantity\nStock: " + stock.QuantityString, "Ok");
                                entry.Text = "";
                            }
                        }
                    }
                    else if (item.Type == ItemType.Labour)
                    {
                        double quan = double.Parse(entry.Text, System.Globalization.CultureInfo.InvariantCulture);
                        if (quan < 0)
                        {
                            await Dialog.Show("Warning", "Quantity Cannot Be A Minus Number", "Ok");
                            entry.Text = "";
                        }
                        else
                        {
                            item.Quantity = quan;
                        }
                    }
                }

                for (int i = 0; i < items.Count; i++)
                {
                    if(items[i].ItemNumber == item.ItemNumber)
                    {
                        items[i] = item;
                        break;
                    }
                }

                listItems.ItemsSource = null;
                listItems.ItemsSource = items;

                CalTotal();
                active = false;
            }
        }

        private void CalTotal()
        {
            if (items.Count > 0)
            {
                double total = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    total += items[i].TotalPrice;
                }
                txtTotalPrice.Text = "£" + Math.Round(total, 2).ToString();
            }
            else
            {
                txtTotalPrice.Text = "£0.00";
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            FirstLoaderPopup();
            string phoneNumber = txtPhoneNumber.Text;
            string email = txtEmail.Text;

            var result = await controller.CheckValues(company, items, phoneNumber, email);
            if(!result)
            {
                ClosePopup();
                return;
            }

            await controller.AddOrder(user, company, items, phoneNumber, email, txtTotalPrice.Text);

            ClosePopup();
            await Navigation.PopAsync();
        }

        private async void btnAddItem_Clicked(object sender, EventArgs e)
        {
            addItemFrame.IsVisible = true;
            MainContent.Opacity = 0.3; //set the main grid opacity to low
            MainContent.InputTransparent = true; //set the main grid not touchable
            AddItemPopup popup = new AddItemPopup(company, items);
            frameAddItemContentView.Content = popup;

            var result = await popup.GetItem();
            if (result != null)
            {
                items.Add(result);
                listItems.ItemsSource = null;
                listItems.ItemsSource = items;
            }

            addItemFrame.IsVisible = false;
            MainContent.Opacity = 1; //make back the opacity of main grid
            MainContent.InputTransparent = false; //make main grid touchable

            CalTotal();
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

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            listItems.SelectedItem = null;
            var vc = (ViewCell)sender;

            ItemListEntry item = items.Find(a => a.ItemNumber == vc.ClassId);
            if(item == null)
                return;   

            var result = await controller.AreYouSure("Warning", "Are You Sure You Want To Delete The Item: " + item.Name, "Yes", "No");
            if (!result)
                return;

            items.RemoveAll(a => a.ItemNumber == item.ItemNumber);

            listItems.ItemsSource = null;
            if(items.Count > 0)
                listItems.ItemsSource = items;

            CalTotal();
        }
    }
}
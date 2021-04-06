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
    public partial class AddItemPopup : ContentView
    {
        AddItemController controller;
        ItemType type;
        Company company;

        List<ItemListEntry> exclude = new List<ItemListEntry>();

        bool returnItem = false;
        ItemListEntry item = null;

        ItemListEntry curItem = null;
        StockItem curStock = null;

        List<string> stockPath = new List<string>();
        List<StockItem> stockList;

        List<User> users = new List<User>();
        public AddItemPopup(Company company, List<ItemListEntry> exclude)
        {
            type = ItemType.Basket;
            this.company = company;
            this.exclude = exclude;
            controller = new AddItemController();
            InitializeComponent();
            Setup();            
        }

        private async void Setup()
        {
            stockList = await controller.GetAllStockItems(company, exclude);
            BasketList();
        }

        public Task<ItemListEntry> GetItem()
        {
            return Task.Run(() =>
             {
                 do
                 {
                     if(returnItem)
                     {
                         return item;
                     }
                 } while (true);
             });
        }

        private void btnBasketAdd_Clicked(object sender, EventArgs e)
        {
            string quantity = quantityBasketEntry.Text;
            if(string.IsNullOrWhiteSpace(quantityBasketEntry.Text))
            {
                Dialog.Show("Warning", "Please Enter A Quantity", "Ok");
                return;
            }
            else
            {
                quantity = quantity.Trim();
            }

            var result = controller.CheckQuantity(quantity);
            if(!result)
            { return; }

            if (int.Parse(quantityBasketEntry.Text.Trim()) > curStock.Quantity)
            {
                Dialog.Show("Warning", "Please Enter A Lower Quantity As There Is Not Enough Stock", "Ok");
                return;
            }

            ItemListEntry temp = new ItemListEntry();
            temp.Name = curStock.Name;
            temp.Amount = curStock.Price;
            temp.Quantity = int.Parse(quantityBasketEntry.Text.Trim());
            temp.ItemNumber = curStock.StockNumber;
            item = temp;
            returnItem = true;
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            liststock.SelectedItem = null;
            var vc = (ViewCell)sender;

            StockItem stock = stockList.Find(a => a.StockNumber == vc.ClassId);
            if(stock.Type == StockType.Category)
            {
                stockPath.Add(stock.Name);
                RefreshBasketList(stock.Name);
            }
            else
            {
                BasketInfo(stock);
            }
        }

        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            listLabour.SelectedItem = null;
            var vc = (ViewCell)sender;

            ItemListEntry itemListEntry = await controller.GetItem(company, vc.ClassId);
            LabourInfo(itemListEntry);
        }

        private void btnLabourAdd_Clicked(object sender, EventArgs e)
        {
            string perHour = perHourEntry.Text;
            if (string.IsNullOrWhiteSpace(perHour))
            {
                return;
            }
            else
            {
                perHour = perHour.Trim();
            }
            var result = controller.CheckPerHour(perHourEntry.Text.Trim());
            if (!result)
            {
                return;
            }

            curItem.Type = ItemType.Labour;
            curItem.Quantity = double.Parse(perHour, System.Globalization.CultureInfo.InvariantCulture);
            item = curItem;
            returnItem = true;
        }

        private void btnType_Clicked(object sender, EventArgs e)
        {
            if(type == ItemType.Basket)
            {
                LabourList();
            }
            else
            {
                BasketList();        
            }
        }

        private void BasketList()
        {
            curStock = null;
            txtTitle.Text = "Stock";
            btnType.Style = Application.Current.Resources["BasketImage"] as Style;

            type = ItemType.Basket;

            labourLayout.IsVisible = false;
            lstLabourLayout.IsVisible = false;
            infoLabourLayout.IsVisible = false;

            basketLayout.IsVisible = true;
            lstBasketLayout.IsVisible = true;
            infoBasketLayout.IsVisible = false;

            string category;
            if(stockPath.Count == 0)
            { category = ""; }
            else
            { category = stockPath[stockPath.Count - 1]; }

            List<StockItem> temp = new List<StockItem>();
            for (int i = 0; i < stockList.Count; i++)
            {
                if (stockList[i].Catergory == category)
                {
                    temp.Add(stockList[i]);
                }
            }
            liststock.ItemsSource = null;
            liststock.ItemsSource = temp;
        }

        private void BasketInfo(StockItem stockItem)
        {
            labourLayout.IsVisible = false;
            lstLabourLayout.IsVisible = false;
            infoLabourLayout.IsVisible = false;

            basketLayout.IsVisible = true;
            lstBasketLayout.IsVisible = false;
            infoBasketLayout.IsVisible = true;

            curStock = stockItem;

            txtTitle.Text = stockItem.Name;
            txtBasketPrice.Text = stockItem.Price.ToString();
            txtBasketDescription.Text = stockItem.Description;

            if (stockItem.Quantity == 0)
            {
                quantityBasketEntry.IsEnabled = false;
                btnBasketAdd.IsEnabled = false;
                txtBasketStock.Text = "Out Of Stock";
            }
            else
            {
                quantityBasketEntry.IsEnabled = true;
                btnBasketAdd.IsEnabled = true;
                txtBasketStock.Text = stockItem.Quantity.ToString();
            }

        }

        private void LabourList()
        {
            curItem = null;

            txtTitle.Text = "Labour";
            btnType.Style = Application.Current.Resources["LabourImage"] as Style;

            type = ItemType.Labour;

            basketLayout.IsVisible = false;
            lstBasketLayout.IsVisible = false;
            infoBasketLayout.IsVisible = false;

            labourLayout.IsVisible = true;
            lstLabourLayout.IsVisible = true;
            infoLabourLayout.IsVisible = false;

            listLabour.ItemsSource = null;
            listLabour.ItemsSource = controller.GetLabourList(company, exclude);
        }

        private void LabourInfo(ItemListEntry itemListEntry)
        {
            basketLayout.IsVisible = false;
            lstBasketLayout.IsVisible = false;
            infoBasketLayout.IsVisible = false;

            labourLayout.IsVisible = true;
            lstLabourLayout.IsVisible = false;
            infoLabourLayout.IsVisible = true;

            curItem = itemListEntry;

            txtTitle.Text = itemListEntry.Name;
            txtLabourPrice.Text = itemListEntry.Amount.ToString();
        }

        private void perHourEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(perHourEntry.Text))
            {
                txtLabourTotal.Text = "£" + "0";
                return;
            }
            var result = controller.CheckPerHour(perHourEntry.Text);
            if(!result)
            {
                txtLabourTotal.Text = "£" + "0";
                return;
            }

            txtLabourTotal.Text = "£" + controller.Multiply(txtLabourPrice.Text, perHourEntry.Text).ToString();
        }

        private void btnLstBack_Clicked(object sender, EventArgs e)
        {
            if(curStock != null)
            {
                BasketList();
            }
            else if(curItem != null)
            {
                LabourList();
            }
            else
            {
                if(type == ItemType.Basket)
                {
                    if(stockPath.Count == 1)
                    {
                        stockPath.RemoveAt(stockPath.Count - 1);
                        RefreshBasketList("");
                        return;
                    }
                    else if( stockPath.Count > 1)
                    {
                        stockPath.RemoveAt(stockPath.Count - 1);
                        RefreshBasketList(stockPath[stockPath.Count - 1]);
                        return;
                    }
                }

                item = null;
                returnItem = true;
                this.IsVisible = false;
            }
        }

        private void RefreshBasketList(string category)
        {
            List<StockItem> temp = new List<StockItem>();
            for (int i = 0; i < stockList.Count; i++)
            {
                if (stockList[i].Catergory.Equals(category))
                {
                    temp.Add(stockList[i]);
                }
            }
            liststock.ItemsSource = null;
            liststock.ItemsSource = temp;            
        }

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }
    }
}
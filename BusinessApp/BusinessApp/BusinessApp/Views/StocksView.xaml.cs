using BusinessApp.Models;
using System;
using System.Collections.Generic;
using BusinessApp.Controllers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BusinessApp.Utilities;

namespace BusinessApp.Views
{
    public enum SelectMode { Single, Multi}
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StocksView : ContentPage
    {
        StocksController controller;
        Company company;
        User user;

        bool setup = false;

        SelectMode mode = SelectMode.Single;
        List<StockItem> multiSelectionList;

        List<StockItem> items;
        List<StockItem> stockPath = new List<StockItem>();
        List<StockItem> lstItems;
        public StocksView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new StocksController();
            InitializeComponent();
            if (user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access == 1)
            {
                btnLog.IsEnabled = false;
                btnLog.BackgroundColor = Color.Transparent;
                btnLog.BorderColor = Color.Transparent;
            }
        }

        protected override void OnAppearing()
        {
            if (setup)
            {
                Refresh();
            }
            else
            {
                Setup();
            }
        }

        private async void Refresh()
        {
            FirstLoaderPopup();
            items = await controller.GetStocks(company);
            RefreshList();
        }

        private async void Setup()
        {
            FirstLoaderPopup();

            items = await controller.GetStocks(company);

            liststock.ItemsSource = null;
            if (items.Count > 0)
            {
                lstItems = new List<StockItem>();
                txtTitle.Text = "Stocks";
                for (int i = 0; i < items.Count; i++)
                {
                    if (string.IsNullOrEmpty(items[i].Catergory))
                    {
                        lstItems.Add(items[i]);
                    }
                }
                liststock.ItemsSource = lstItems;
            }
            else
            { liststock.ItemsSource = new List<StockItem>() { new StockItem() { Name = "No Stock", Type = StockType.Category } }; }

            if (user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access == 1)
            {
                btnLog.IsVisible = false;
                btnDelete.IsVisible = false;
            }
            else
            {
                btnLog.IsVisible = true;
                btnDelete.IsVisible = true;
            }

            ClosePopup();
            setup = true;
        }

        private void RefreshList()
        {
            liststock.ItemsSource = null;
            if (items.Count > 0)
            {
                if (stockPath.Count == 0)
                {
                    txtTitle.Text = "Stocks";
                    lstItems = controller.GetStockInCategory(items, "");
                }
                else
                {
                    txtTitle.Text = stockPath[stockPath.Count - 1].Name;
                    lstItems = controller.GetStockInCategory(items, stockPath[stockPath.Count - 1].Name);
                }

                if (lstItems.Count > 0)
                {
                    ClosePopup();
                    liststock.ItemsSource = lstItems;
                    return;
                }
            }

            liststock.ItemsSource = new List<StockItem>() { new StockItem() { Name = "No Items In This Catergory", Type = StockType.Category } };

            ClosePopup();
        }

        private void btnAddStock_Clicked(object sender, EventArgs e)
        {
            if (stockPath.Count == 0)
                Navigation.PushAsync(new AddStockView(user, company, ""));
            else
                Navigation.PushAsync(new AddStockView(user, company, stockPath[stockPath.Count - 1].Name));
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var vc = ((ViewCell)sender);
            liststock.SelectedItem = null;

            if (lstItems == null)
                return;
            if (lstItems.Count == 0)
                return;

            if (mode == SelectMode.Single)
            {
                StockItem item = lstItems.Find(a => a.StockNumber == vc.ClassId);

                if (item.Type == StockType.Item)
                {
                    Navigation.PushAsync(new StockInfoView(user, company, item));
                }
                else
                {
                    stockPath.Add(item);
                    RefreshList();
                }
            }
            else
            {
                StockItem item = lstItems.Find(a => a.StockNumber == vc.ClassId);
                bool catHasItems = false;
                if (item.Type == StockType.Category)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (item.Name == items[i].Catergory)
                        {
                            catHasItems = true;
                            Dialog.Show("Warning", "This Catergory currently has items within it, please delete items before deleting catergory", "Ok");
                            break;
                        }
                    }
                }

                if (!catHasItems)
                {
                    if (item.Selected)
                    {
                        if (multiSelectionList.Count > 0)
                            multiSelectionList.Remove(item);
                        item.Selected = false;
                    }
                    else
                    {
                        multiSelectionList.Add(item);
                        item.Selected = true;
                    }
                    liststock.ItemsSource = null;
                    liststock.ItemsSource = lstItems;
                }
            }
        }

        public void ToggleMultiSelection()
        {
            if (mode == SelectMode.Multi)
            {
                mode = SelectMode.Single;

                if(multiSelectionList != null)
                    multiSelectionList.Clear();

                if (lstItems.Count > 0)
                {
                    foreach (StockItem item in lstItems)
                    {
                        item.Selected = false;
                        item.SelectionMode = false;
                    }
                }

                liststock.ItemsSource = null;
                liststock.ItemsSource = lstItems;
            }
            else
            {
                mode = SelectMode.Multi;
                multiSelectionList = new List<StockItem>();
                if (lstItems.Count > 0)
                {
                    foreach (StockItem item in lstItems)
                    {
                        item.SelectionMode = true;
                    }
                }

                liststock.ItemsSource = null;
                liststock.ItemsSource = lstItems;
            }
        }

        public async void DeleteItems()
        {            
            FirstLoaderPopup();
            if (multiSelectionList.Count > 0)
            {
                var result = await controller.DeleteItems(user, company, multiSelectionList);
                if (result)
                {
                    ToggleMultiSelection();
                    Refresh();
                }
                else
                {
                    ClosePopup();
                }
            }
            else
            {
                ToggleMultiSelection();
                ClosePopup();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Back();
            return true;
        }

        private void Back()
        {
            if (mode == SelectMode.Single)
            {
                if (stockPath.Count > 0)
                {
                    stockPath.RemoveAt(stockPath.Count - 1);
                    RefreshList();
                }
                else
                {
                    Navigation.PopAsync();
                }
            }
            else
            {
                ToggleMultiSelection();
            }
        }

        private void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (lstItems == null)
                return;
            if (lstItems.Count == 0)
                return;
            if (mode == SelectMode.Multi)
            {
                DeleteItems();
            }
            else
            {
                ToggleMultiSelection();
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

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Back();
        }

        private async void btnLog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StockLogsView(user, company));
        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            Refresh();
        }

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }
    }
}
using BusinessApp.Controllers;
using BusinessApp.Models;
using BusinessApp.Utilities;
using OxyPlot;
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
    public partial class ProfitsView : ContentPage
    {
        ProfitsController controller;
        User user;
        Company company;

        List<Order> orders = new List<Order>();
        List<StockItem> stocks = new List<StockItem>(0);
        bool setup = false;

        PlotModel Model;
        public ProfitsView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new ProfitsController();
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            LoadingPopup();

            orders = await controller.GetOrders(company);
            stocks = await controller.GetStocks(company);

            List<string> temp = new List<string>();
            yearPicker.Items.Clear();
            if (orders != null)
            {
                if (orders.Count > 0)
                {
                    for (int i = 0; i < orders.Count; i++)
                    {
                        if (!temp.Contains(orders[i].Date.Year.ToString()))
                        {
                            temp.Add(orders[i].Date.Year.ToString());
                        }
                    }
                    yearPicker.ItemsSource = temp;
                    for (int i = 0; i < yearPicker.Items.Count; i++)
                    {
                        if (yearPicker.Items[i] == DateTime.Now.Year.ToString())
                        {
                            yearPicker.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    yearPicker.Items.Add(DateTime.Now.Year.ToString());
                    yearPicker.SelectedIndex = 0;
                }
            }
            else
            {
                yearPicker.Items.Add(DateTime.Now.Year.ToString());
                yearPicker.SelectedIndex = 0;
            }

            setup = true;
            UpdateGraph();
        }

        private void UpdateGraph()
        {
            LoadingPopup();

            int year = int.Parse(yearPicker.SelectedItem.ToString());

            double[] values = new double[12];

            if (orders.Count > 0)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].Date.Year == year)
                    {
                        double cost = 0;
                        double earnings = 0;
                        for (int j = 0; j < orders[i].Items.Count; j++)
                        {
                            if (orders[i].Items[j].Type == ItemType.Basket)
                            {
                                StockItem stock = stocks.Find(a => a.StockNumber == orders[i].Items[j].ItemNumber);
                                cost += stock.Cost * orders[i].Items[j].Quantity;
                                earnings += stock.Price * orders[i].Items[j].Quantity;
                            }
                            else
                            {
                                earnings += orders[i].Items[j].TotalPrice;
                            }
                        }
                        values[orders[i].Date.Month - 1] += (earnings - cost);
                    }
                }
            }
            else
            {
                ClosePopup();
                return;
            }

            Model = controller.CreateBarChart(false, values);
            viewBarChart.Model = Model;

            ClosePopup();
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

        private void yearPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
                return;
            UpdateGraph();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "allowLandScape");
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "preventLandScape");
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
    }
}
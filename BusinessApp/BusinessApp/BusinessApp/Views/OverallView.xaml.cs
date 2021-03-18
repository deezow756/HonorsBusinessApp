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
    public enum OverallSection { Month, Year}
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverallView : ContentPage
    {
        OverallController controller;
        User user;
        Company company;

        List<Order> orders = new List<Order>();
        List<StockItem> stocks = new List<StockItem>();
        bool setup = false;
        bool yearChanged = true;

        public OverallView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new OverallController();
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            LoadingPopup();

            orders = await controller.GetOrders(company);
            stocks = await controller.GetStocks(company);

            List<string> temp = new List<string>();
            monthPicker.SelectedIndex = DateTime.Now.Month - 1;
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
                    yearPicker.ItemsSource = null;
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
            yearChanged = true;
            RefreshDetails();
        }

        private void RefreshDetails()
        {
            LoadingPopup();

            controller.RefreshDetails(OverallSection.Month, txtMonthCost, txtMonthEarnings, txtMonthProfit, orders, stocks, monthPicker.SelectedIndex + 1, int.Parse(yearPicker.Items[yearPicker.SelectedIndex]));

            if(yearChanged)
            {
                controller.RefreshDetails(OverallSection.Year, txtYearCost, txtYearEarnings, txtYearProfit, orders, stocks, monthPicker.SelectedIndex + 1, int.Parse(yearPicker.Items[yearPicker.SelectedIndex]));
                yearChanged = false;
            }

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

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            setup = false;
            Setup();
        }

        private void btnLeft_Clicked(object sender, EventArgs e)
        {
            if (monthPicker.SelectedIndex > 0)
            {
                monthPicker.SelectedIndex = monthPicker.SelectedIndex - 1;
                RefreshDetails();
            }
            else if (monthPicker.SelectedIndex == 0)
            {
                if (yearPicker.SelectedIndex > 0)
                {
                    yearChanged = true;
                    yearPicker.SelectedIndex = yearPicker.SelectedIndex - 1;
                    monthPicker.SelectedIndex = 11;
                    RefreshDetails();
                }
            }
        }

        private void btnRight_Clicked(object sender, EventArgs e)
        {
            if (monthPicker.SelectedIndex < 11)
            {
                monthPicker.SelectedIndex = monthPicker.SelectedIndex + 1;
                RefreshDetails();
            }
            else if (monthPicker.SelectedIndex == 11)
            {
                if (yearPicker.SelectedIndex < yearPicker.Items.Count - 1)
                {
                    yearChanged = true;
                    yearPicker.SelectedIndex = yearPicker.SelectedIndex + 1;
                    monthPicker.SelectedIndex = 0;
                    RefreshDetails();
                }
            }
        }

        private void monthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
                return;
            RefreshDetails();
        }

        private void yearPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
                return;
            yearChanged = true;
            RefreshDetails();
        }

        private void LoadingPopup()
        {
            arcFrame.IsVisible = true; //visible the frame
            Scroll.ScrollToAsync(arcFrame, ScrollToPosition.Center, true); //scrolls so that the frame is at the center of the list
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
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
    public enum FilterMode { Date, Search}
    public enum SearchType { None, OrderNumber, Email, ContactNumber}
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersView : ContentPage
    {
        User user;
        Company company;
        OrdersController controller;
        bool setup = false;
        FilterMode mode = FilterMode.Date;

        List<Order> orders = new List<Order>();
        SearchType type = SearchType.None;
        public OrdersView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new OrdersController();
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            orders = await controller.GetAllOrders(company);
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

            if(user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access == 1)
            {
                btnLog.IsVisible = false;
                btnDelete.IsVisible = false;
            }
            else
            {
                btnLog.IsVisible = true;
                btnDelete.IsVisible = true;
            }

            setup = true;

            RefreshList();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(setup)
            {
                orders = await controller.GetAllOrders(company);
                RefreshList();
            }
        }

        private void RefreshList()
        {
            lstOrders.ItemsSource = null;
            List<Order> temp = new List<Order>();
            if(mode == FilterMode.Date)
            {
                temp = controller.FilterListByDate(orders, monthPicker.SelectedIndex + 1, int.Parse(yearPicker.Items[yearPicker.SelectedIndex]));
            }
            else if(mode == FilterMode.Search)
            {
                temp = controller.FilterListBySearch(type, orders, entrySearch.Text);
            }
            lstOrders.ItemsSource = temp;
        }

        private async void btnRefresh_Clicked(object sender, EventArgs e)
        {
            orders = await controller.GetAllOrders(company);
            RefreshList();
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

        private void btnLeft_Clicked(object sender, EventArgs e)
        {
            if(monthPicker.SelectedIndex > 0)
            {
                monthPicker.SelectedIndex = monthPicker.SelectedIndex - 1;
                RefreshList();
            }
            else if(monthPicker.SelectedIndex == 0)
            {
                if (yearPicker.SelectedIndex > 0)
                {
                    yearPicker.SelectedIndex = yearPicker.SelectedIndex - 1;
                    monthPicker.SelectedIndex = 11;
                    RefreshList();
                }
            }
        }

        private void btnRight_Clicked(object sender, EventArgs e)
        {
            if(monthPicker.SelectedIndex < 11)
            {
                monthPicker.SelectedIndex = monthPicker.SelectedIndex + 1;
                RefreshList();
            }
            else if (monthPicker.SelectedIndex == 11)
            {
                if (yearPicker.SelectedIndex < yearPicker.Items.Count - 1)
                {
                    yearPicker.SelectedIndex = yearPicker.SelectedIndex + 1;
                    monthPicker.SelectedIndex = 0;
                    RefreshList();
                }
            }
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            lstOrders.SelectedItem = null;
            var vc = (ViewCell)sender;
            Order order = orders.Find(a => a.OrderNumber == vc.ClassId);
            if(order != null)
            {
                await Navigation.PushAsync(new OrderInfoView(user, company, order));
            }
        }

        private void btnSearch_Clicked(object sender, EventArgs e)
        {
            string temp = entrySearch.Text;
            if(string.IsNullOrWhiteSpace(temp))
            {
                Dialog.Show("Warning", "Please Enter Text In The Search Bar", "Ok");
                return;
            }
            if(searchPicker.SelectedIndex == 0)
            {
                Dialog.Show("Warning", "Please Select What You Would Like to Search By", "Ok");
                return;
            }

            mode = FilterMode.Search;
            RefreshList();
        }

        private void monthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
                return;
            mode = FilterMode.Date;
            RefreshList();
        }

        private void yearPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
                return;
            mode = FilterMode.Date;
            RefreshList();
        }

        private void searchPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(searchPicker.SelectedIndex == 0)
            {
                type = SearchType.None;
            }
            else if (searchPicker.SelectedIndex == 1)
            {
                type = SearchType.OrderNumber;
            }
            else if (searchPicker.SelectedIndex == 2)
            {
                type = SearchType.Email;
            }
            else if (searchPicker.SelectedIndex == 3)
            {
                type = SearchType.ContactNumber;
            }
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            entrySearch.Text = "";
        }

        private async void btnLog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderLogsView(user, company));
        }

        private async void btnAddStock_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrderView(user, company));
        }

        private void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entrySearch.Text))
            {
                mode = FilterMode.Date;
                RefreshList();
            }
        }
    }
}
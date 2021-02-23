using BusinessApp.Controllers;
using BusinessApp.Models;
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
    public partial class OrderLogsView : ContentPage
    {
        OrderLogsController controller;
        private User user;
        private Company company;
        private List<OrderLog> orders = new List<OrderLog>();
        private List<OrderLog> filteredOrders = new List<OrderLog>();
        private string searchString = "";

        private bool filterMonth = true;
        private bool filterYear = true;

        private bool setup = false;

        public OrderLogsView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new OrderLogsController();
            InitializeComponent();
            Setup();
        }

        private async void Refresh()
        {
            orders = await controller.GetAllOrderLogs(company);
            RefreshList();
        }

        private async void Setup()
        {
            orders = await controller.GetAllOrderLogs(company);
            List<string> temp = new List<string>();
            monthPicker.SelectedIndex = DateTime.Now.Month - 1;
            yearPicker.SelectedIndex = -1;
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

            RefreshList();
        }

        private void RefreshList()
        {
            lstLogs.ItemsSource = null;
            if (orders.Count > 0)
            {
                filteredOrders = orders;
                if (filterYear)
                {
                    filteredOrders = controller.FilterList(DateMode.Year, filteredOrders, int.Parse(yearPicker.Items[yearPicker.SelectedIndex]));
                }
                if (filteredOrders.Count == 0)
                {
                    lstLogs.ItemsSource = new List<ManagerLog>() { new ManagerLog() { Message = "No Logs Found With Filter" } };
                    return;
                }
                if (filterMonth)
                {
                    filteredOrders = controller.FilterList(DateMode.Month, filteredOrders, monthPicker.SelectedIndex + 1);
                }
                if (filteredOrders.Count == 0)
                {
                    lstLogs.ItemsSource = new List<ManagerLog>() { new ManagerLog() { Message = "No Logs Found With Filter" } };
                    return;
                }
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    filteredOrders = controller.FilterList(filteredOrders, searchString);
                }

                lstLogs.ItemsSource = null;
                if (filteredOrders.Count == 0)
                {
                    lstLogs.ItemsSource = new List<ManagerLog>() { new ManagerLog() { Message = "No Logs Found With Filter" } };
                }
                else
                {
                    lstLogs.ItemsSource = filteredOrders;
                }
            }
            else
            {
                lstLogs.ItemsSource = new List<ManagerLog>() { new ManagerLog() { Message = "No Logs Found" } };
            }
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

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            Refresh();
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            searchEntry.Text = "";
            searchString = "";
            Refresh();
        }

        private void btnRadioMonth_Clicked(object sender, EventArgs e)
        {
            if (filterMonth)
            {
                filterMonth = false;
                btnRadioMonth.Style = Application.Current.Resources["RadioUnchecked"] as Style;
            }
            else
            {
                filterMonth = true;
                btnRadioMonth.Style = Application.Current.Resources["RadioChecked"] as Style;
            }

            Refresh();
        }

        private void btnRadioYear_Clicked(object sender, EventArgs e)
        {
            if (filterYear)
            {
                filterYear = false;
                btnRadioYear.Style = Application.Current.Resources["RadioUnchecked"] as Style;
            }
            else
            {
                filterYear = true;
                btnRadioYear.Style = Application.Current.Resources["RadioChecked"] as Style;
            }

            Refresh();
        }

        private void searchEntry_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!setup)
            { return; }
            if (string.IsNullOrWhiteSpace(searchEntry.Text))
            {
                searchString = "";
            }
            else
            {
                searchString = searchEntry.Text;
            }

            Refresh();
        }

        private void monthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
            { return; }
            if (monthPicker.SelectedIndex > -1)
            {
                Refresh();
            }
        }

        private void yearPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
            { return; }
            if (yearPicker.SelectedIndex > -1)
            {
                Refresh();
            }
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            lstLogs.SelectedItem = null;
        }
    }
}
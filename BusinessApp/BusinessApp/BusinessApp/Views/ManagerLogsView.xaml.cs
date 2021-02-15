using BusinessApp.Models;
using BusinessApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusinessApp.Views
{
    public enum DateMode { Month, Year}
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManagerLogsView : ContentPage
    {
        ManagerLogsController controller;
        private User user;
        private Company company;
        private List<ManagerLog> logs = new List<ManagerLog>();
        private List<ManagerLog> filteredLogs = new List<ManagerLog>();
        private string searchString = "";

        private bool filterMonth = true;
        private bool filterYear = true;

        private bool setup = false;

        public ManagerLogsView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new ManagerLogsController();
            InitializeComponent();            
            Setup();
        }

        private async void Refresh()
        {
            logs = await controller.GetAllManagerLogs(company);
            RefreshList();
        }

        private async void Setup()
        {
            logs = await controller.GetAllManagerLogs(company);
            List<string> temp = new List<string>();
            monthPicker.SelectedIndex = DateTime.Now.Month - 1;
            yearPicker.SelectedIndex = -1;
            yearPicker.Items.Clear();
            if (logs != null)
            {
                if (logs.Count > 0)
                {
                    for (int i = 0; i < logs.Count; i++)
                    {
                        if (!temp.Contains(logs[i].Date.Year.ToString()))
                        {
                            temp.Add(logs[i].Date.Year.ToString());
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
            if (logs.Count > 0)
            {
                filteredLogs = logs;
                if (filterYear)
                {
                    filteredLogs = controller.FilterList(DateMode.Year, filteredLogs, int.Parse(yearPicker.Items[yearPicker.SelectedIndex]));
                }

                if (filterMonth)
                {
                    filteredLogs = controller.FilterList(DateMode.Month, filteredLogs, monthPicker.SelectedIndex + 1);
                }

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    filteredLogs = controller.FilterList(filteredLogs, searchString);
                }

                lstLogs.ItemsSource = null;
                if (filteredLogs.Count == 0)
                {
                    lstLogs.ItemsSource = new List<ManagerLog>() { new ManagerLog() { Message = "No Logs Found With Filter" } };
                }
                else
                {                    
                    lstLogs.ItemsSource = filteredLogs;
                }
            }
            else
            {
                lstLogs.ItemsSource = null;
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
            if(filterMonth)
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
            if(!setup)
            { return; }
            if(monthPicker.SelectedIndex > -1)
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
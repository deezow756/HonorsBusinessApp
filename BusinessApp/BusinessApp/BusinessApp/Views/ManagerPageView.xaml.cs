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
    public partial class ManagerPageView : ContentPage
    {
        private bool pageOpen;
        ManagerController controller;
        User user;
        Company company;
        List<User> users;
        List<User> requestUsers;

        public ManagerPageView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new ManagerController();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageOpen = true;
            FirstLoaderPopup();
            CheckAccess();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageOpen = false;
        }

        private void Refresh()
        {
            FillUsersList();
        }

        private void FillUsersList()
        {
            requestsLayout.IsVisible = false;

            requestUsers = controller.GetRequests(company);
            if (requestUsers.Count > 0)
            {
                requestsLayout.IsVisible = true;
                lstRequests.ItemsSource = null;
                lstRequests.ItemsSource = requestUsers;
            }
            else
            {
                requestsLayout.IsVisible = false;
            }

            users = controller.GetEmployees(company);
            lstEmployees.ItemsSource = null;
            lstEmployees.ItemsSource = users;
            ClosePopup();
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

        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            var vc = (ViewCell)sender;

            lstRequests.SelectedItem = null;
            User temp = requestUsers.Find(a => a.Email == vc.ClassId);
            if (temp != null)
                await Navigation.PushAsync(new EmployeeRequestView(user, temp, company));
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            var vc = (ViewCell)sender;

            lstEmployees.SelectedItem = null;
            User temp = users.Find(a => a.Email == vc.ClassId);
            if (temp != null)
                await Navigation.PushAsync(new EmployeeView(user, temp, company));
        }

        private async void btnLogs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManagerLogsView(user, company));
        }

        private async void btnCompanyProfile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompanyProfileView(user, company));
        }

        private async Task CheckAccess()
        {
            Task.Run(Check);

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
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
            company = await controller.GetCompany(company.CompanyNumber);

            User temp = company.Employees.Find(a => a.AccountCreated == user.AccountCreated && a.Email == user.Email);
            if (temp != null)
            {
                if (temp.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access < 2)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Dialog.Show("Warning", "You No Longer Have Access To Manager Features", "Ok");
                        await Navigation.PopAsync();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Refresh();
                    });
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Dialog.Show("Warning", "You Have Been Removed From Company", "Ok");
                    await Navigation.PopAsync();
                });
            }
        }

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.Displayhelp();
        }
    }
}
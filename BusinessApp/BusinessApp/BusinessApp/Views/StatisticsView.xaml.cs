using BusinessApp.Models;
using BusinessApp.Utilities;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsView : ContentPage
    {
        StatisticsController controller;
        User user;
        Company company;
        bool pageOpen = false;

        public StatisticsView(User user, Company company)
        {
            this.user = user;
            this.company = company;
            controller = new StatisticsController();
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            double businessValue = await controller.CalculateBusinessValue(company);
            txtBusinessValue.Text = "£" + businessValue.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageOpen = true;
            CheckAccess();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageOpen = false;
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnProfits_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfitsView(user, company));
        }

        private async void btnOverall_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OverallView(user, company));
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
    }
}
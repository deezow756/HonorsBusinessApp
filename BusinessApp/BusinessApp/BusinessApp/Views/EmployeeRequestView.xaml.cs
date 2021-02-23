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
    public partial class EmployeeRequestView : ContentPage
    {
        EmployeeRequestController controller;
        User editor;
        User user;
        Company company;
        public EmployeeRequestView(User editor, User user, Company company)
        {
            this.editor = editor;
            this.user = user;
            this.company = company;
            controller = new EmployeeRequestController();
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            txtName.Text = user.Name;
            txtEmail.Text = user.Email;

            pickerRoles.ItemsSource = controller.GetRolesString(company.Roles);
        }

        private void pickerManagerAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            if(picker.SelectedIndex == 0)
            {
                user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access = 2;
            }
            else
            {
                user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access = 1;
            }
        }

        private void pickerRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).CurrentRole = new Role() { Name = picker.SelectedItem.ToString() };
        }

        private async void btnAccept_Clicked(object sender, EventArgs e)
        {
            string rate = entryHourlyRate.Text;
            if(string.IsNullOrWhiteSpace(rate))
            {
                Dialog.Show("Warning", "Please Enter A Hourly Rate", "Ok");
                return; 
            }
            user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).HourlyRate = double.Parse(rate, System.Globalization.CultureInfo.InvariantCulture);
            var result = controller.CheckHourlyRate(rate);
            if(!result)
            {
                return;
            }
            result = controller.CheckCompanyID(user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber));
            if(!result)
            {
                return;
            }

            await controller.AcceptRequest(editor, user, company);

            await Navigation.PopAsync();
        }

        private async void btnReject_Clicked(object sender, EventArgs e)
        {
            var result = await controller.AreYouSure();
            if(!result)
            {
                return;
            }

            await controller.RejectRequest(editor, user, company);

            await Navigation.PopAsync();
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
    }
}
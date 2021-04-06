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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderInfoView : ContentPage
    {
        User user;
        Company company;
        OrderInfoController controller;
        Order order;
        public OrderInfoView(User user, Company company, Order order)
        {
            this.user = user;
            this.company = company;
            this.order = order;
            controller = new OrderInfoController();
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            txtOrderNumber.Text = order.OrderNumberString;
            txtDate.Text = order.DateString;

            if(order.Email != null)
            {
                txtEmail.IsVisible = true;
                txtEmail.Text = order.Email;
            }
            if(order.ContactNumber != null)
            {
                txtContactNumber.IsVisible = true;
                txtContactNumber.Text = order.ContactNumber;
            }
            
            if(order.Paid)
            {
                txtPaid.Text = "Paid";
                btnPaid.IsVisible = false;
            }
            else
            {
                txtPaid.Text = "Unpaid";
            }

            txtTotal.Text = order.TotalString;

            lstItems.ItemsSource = null;
            lstItems.ItemsSource = order.Items;

            order.CompanyNumber = company.CompanyNumber;
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

        private async void btnPaid_Clicked(object sender, EventArgs e)
        {
            var result = await controller.AreYouSure("Warning", "Are You Sure You Want To Change The Paid Status Of The Order To Paid?", "Yes", "No");
            if(!result)
            {
                return;
            }

            await controller.ChangePaidStatus(company, order);
            order.Paid = true;
            Setup();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            lstItems.SelectedItem = null;
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            LoadingPopup();
            string action = await DisplayActionSheet("Choose How You Would Like To Delete The Order:\nDelete Order And..", "Cancel", null, "Return Item Stock", "Dont Return Item Stock");

            if (action == null || action == "Cancel")
            {
                ClosePopup();
                return;
            }

            var result = await controller.AreYouSure("Warning", "Are You Sure You Want To Delete This Order?", "Yes", "No");
            if (!result)
            {
                ClosePopup();
                return;
            }

            if (action == "Dont Return Item Stock")
            {
                await controller.DeleteOrder(user, company, order);
            }
            else if(action == "Return Item Stock")
            {
                await controller.DeleteOrder(user, company, order, true);
            }

            ClosePopup();
            await Navigation.PopAsync();
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

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            controller.DisplayHelp();
        }
    }
}
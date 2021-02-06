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
    public enum Mode { View, Edit}
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileView : ContentPage
    {
        private User user;
        private ProfileController controller;
        private Mode mode;

        private bool firstNameChanged = false;
        private bool surnameChanged = false;
        private bool emailChanged = false;
        public ProfileView(User user)
        {
            this.user = user;
            controller = new ProfileController();
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            DisplayView();
        }

        private async void RefreshView()
        {
            user = await controller.GetUser(txtEntryEmail.Text.Trim());
            DisplayView();
        }

        private async void RefreshEdit()
        {
            user = await controller.GetUser(txtEntryEmail.Text.Trim());
            DisplayEdit();
        }

        private async void DisplayView()
        {
            mode = Mode.View;
            nonEditLayout.IsVisible = true;
            editLayout.IsVisible = false;
            txtFirstName.Text = user.FirstName;
            txtSurname.Text = user.Surname;
            txtEmail.Text = user.Email;
            lstCompanies.ItemsSource = null;
            List<Company> companies = await controller.GetCompanies(user);
            if (companies != null)
            {
                lstCompanies.ItemsSource = companies;
            }
        }

        private async void DisplayEdit()
        {
            mode = Mode.Edit;
            nonEditLayout.IsVisible = false;
            editLayout.IsVisible = true;
            txtEntryFirstName.Text = user.FirstName;
            txtEntrySurname.Text = user.Surname;
            txtEntryEmail.Text = user.Email;
            txtEntryCompanyID.Text = "";
            lstCompanies1.ItemsSource = null;
            lstCompanies1.ItemsSource = await controller.GetCompanies(user);
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            lstCompanies.SelectedItem = null;
        }

        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            lstCompanies1.SelectedItem = null;
            var vc = (ViewCell)sender;

            var result = controller.CheckDisconnectEligible(user, vc.ClassId);
            if(!result)
                return;            

            result = await controller.AreYouSure("Warning", "Are You Sure You Want To Disconnect From Company", "Yes", "No");
            if(!result)
                return;

            result = await controller.DisconnectWithCompany(user, vc.ClassId);
            if(result)
                RefreshEdit();            
        }

        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            var result = await controller.ConnectWithCompany(user, txtEntryCompanyID.Text);

            if(result)
            {
                user = await controller.GetUser(user.Email);
                DisplayEdit();
            }
            else
            {
                return;
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if (mode != Mode.Edit)
                return;

            if (!firstNameChanged && !surnameChanged && !emailChanged)
                return;

            LoadingPopup();

            var result = controller.CheckDetails(firstNameChanged, surnameChanged, emailChanged, txtEntryFirstName.Text, txtEntrySurname.Text, txtEntryEmail.Text);

            if(!result)
            {
                ClosePopup();
                return;
            }

            result = await controller.SaveChanges(user, txtEntryFirstName.Text, txtEntrySurname.Text, txtEntryEmail.Text);

            if (result)
            {
                ClosePopup();
                RefreshView();
            }
            else
                ClosePopup();

            firstNameChanged = false;
            surnameChanged = false;
            emailChanged = false;
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

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            if(mode == Mode.View)
            {
                DisplayEdit();
            }
            else
            {
                RefreshView();                
            }
            firstNameChanged = false;
            surnameChanged = false;
            emailChanged = false;
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

        private void txtEntryFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            firstNameChanged = true;
        }

        private void txtEntrySurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            surnameChanged = true;
        }

        private void txtEntryEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            emailChanged = true;
        }
    }
}
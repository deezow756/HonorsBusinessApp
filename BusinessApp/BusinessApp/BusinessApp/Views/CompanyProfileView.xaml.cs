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
    public partial class CompanyProfileView : ContentPage
    {
        CompanyProfileController controller;
        User editor;
        Company company;
        Mode mode;
        public CompanyProfileView(User editor, Company company)
        {
            this.editor = editor;
            this.company = company;
            controller = new CompanyProfileController();
            InitializeComponent();
            LoadingPopup();
            SetView();
        }

        private async void Refresh(Mode mode)
        {
            company = await controller.GetCompany(company.CompanyNumber);
            if (mode == Mode.View)
            { SetView(); }
            else
            { SetEdit(); }
        }

        private void SetView()
        {
            mode = Mode.View;
            viewLayout.IsVisible = true;
            editLayout.IsVisible = false;
            txtCompanyName.Text = company.Name;
            txtCompanyNumber.Text = company.CompanyNumber;
            lstRoles.ItemsSource = null;
            lstRoles.ItemsSource = company.Roles;
            ClosePopup();
        }

        private void SetEdit()
        {
            mode = Mode.Edit;
            viewLayout.IsVisible = false;
            editLayout.IsVisible = true;
            txtEntryCompanyName.Text = company.Name;
            lstRoles1.ItemsSource = null;
            lstRoles1.ItemsSource = company.Roles;
            txtEntryRole.Text = "";
            ClosePopup();
        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            if (mode == Mode.View)
            {
                SetEdit();
            }
            else
            {
                SetView();
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

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            LoadingPopup();
            string compName = txtEntryCompanyName.Text;
            if (string.IsNullOrEmpty(compName))
            {
                ClosePopup();
                Dialog.Show("Warning", "Please Enter A Company Name In The Textbox Provided", "Ok");
                return;
            }
            else
            {
                compName = compName.Trim();
            }

            company.Name = compName;
            await controller.SaveChanges(editor, company);

            Refresh(Mode.View);

            ClosePopup();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            lstRoles.SelectedItem = null;
        }

        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            lstRoles1.SelectedItem = null;
            var vc = (ViewCell)sender;

            if(company.Roles.Count == 1)
            {
                Dialog.Show("Warning", "You Cannot Delete This Role As You Only Have 1 Role\n If You Would Like To Delete This Role, Please Add A New Role First", "Ok");
                return;
            }

            var result = await controller.AreYouSure("Warning", "Are You Sure You Want To Delete This Role: " + ClassId + "\nThis Will Remove The Role From All Employees That Are Assigned This Role.", "Yes", "No");
            if(!result)
            {
                return;
            }

            LoadingPopup();

            Role role = company.Roles.Find(a => a.Name == vc.ClassId);
            await controller.DeleteRole(editor, company, role);

            Refresh(Mode.Edit);

            ClosePopup();
        }

        private async void btnRole_Clicked(object sender, EventArgs e)
        {
            LoadingPopup();
            string role = txtEntryRole.Text;

            if(string.IsNullOrEmpty(role))
            {
                ClosePopup();
                Dialog.Show("Warning", "Please Enter A Role In The Textbox Provided Before Clicking Add Role", "Ok");
                return;
            }

            role = role.Trim();

            await controller.AddRole(editor, company, role);

            Refresh(Mode.Edit);
            ClosePopup();
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
            controller.DisplayHelp(mode);
        }
    }
}
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
    public partial class EmployeeView : ContentPage
    {
        EmployeeController controller;
        User editor;
        User user;
        Company company;
        Mode mode;
        public EmployeeView(User editor, User user, Company company)
        {
            this.editor = editor;
            this.user = user;
            this.company = company;
            controller = new EmployeeController();
            InitializeComponent();
            LoadingPopup();
            SetView();
        }

        private async void Refresh(Mode mode)
        {
            user = await controller.GetUser(user.Email);
            if(mode == Mode.View)
            { SetView(); }
            else
            { SetEdit(); }
        }

        private void SetView()
        {
            mode = Mode.View;
            viewLayout.IsVisible = true;
            editLayout.IsVisible = false;
            txtName.Text = user.Name;
            txtEmail.Text = user.Email;
            txtCompanyId.Text = company.CompanyNumber;
            CompanyID companyID = user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);
            txtRole.Text = companyID.CurrentRole.Name;
            if(companyID.Access == 1)
            {
                txtAccess.Text = "No";
            }
            else if(companyID.Access == 2)
            {
                txtAccess.Text = "Yes";
            }

            ClosePopup();
        }

        private void SetEdit()
        {
            mode = Mode.Edit;
            viewLayout.IsVisible = false;
            editLayout.IsVisible = true;
            CompanyID companyID = user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);

            List<string> roles = controller.GetRolesString(company.Roles);

            pickerRole.ItemsSource = null;
            pickerRole.ItemsSource = roles;

            for (int i = 0; i < roles.Count; i++)
            {
                if(user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).CurrentRole.Name == roles[i])
                {
                    pickerRole.SelectedIndex = i;
                    break;
                }
            }

            if (companyID.Access == 1)
            {
                pickerManager.SelectedIndex = 1;
            }
            else if(companyID.Access == 2)
            {
                pickerManager.SelectedIndex = 0;
            }
            ClosePopup();
        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            if(mode == Mode.View)
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
            user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).CurrentRole.Name = pickerRole.SelectedItem.ToString();
            if(pickerManager.SelectedIndex == 0)
            {
                user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access = 2;
            }
            else
            {
                user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber).Access = 1;
            }

            await controller.SaveChanges(editor, user, company);

            Refresh(Mode.View);

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

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var result = await controller.AreYouSure("Warning", "Are You Sure You Want To Remove " + user.Name + " From The Company?", "Yes", "No");
            if(!result)
            {
                return;
            }

            await controller.RemoveEmployee(editor, user, company);

            await Navigation.PopAsync();
        }
    }
}
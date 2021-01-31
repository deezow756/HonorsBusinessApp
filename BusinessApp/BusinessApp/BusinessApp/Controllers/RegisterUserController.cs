using System;
using System.Collections.Generic;
using System.Text;
using BusinessApp.Interfaces;
using BusinessApp.Utilities;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class RegisterUserController : IRegisterUserController
    {
        public bool CheckRegistrationDetails(ContentPage page, string firstName, string surname, bool match, string email)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                Dialog.Show(page, "Warning", "Please Enter A First Name", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(surname))
            {
                Dialog.Show(page, "Warning", "Please Enter A Surname", "Ok");
                return false;
            }

            if (match == false)
            {
                Dialog.Show(page, "Warning", "Please Enter Valid Password", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                Dialog.Show(page, "Warning", "Please Enter An Email", "Ok");
                return false;
            }

            //FileManager file = new FileManager();
            //List<User> users = await file.GetAllUsers();
            //if (users != null)
            //{
            //    if (users.Count > 0)
            //    {
            //        for (int i = 0; i < users.Count; i++)
            //        {
            //            if (users[i].Email == tempEmail)
            //            {
            //                ClosePopup();
            //                await DisplayAlert("Warning", "Email Already Used", "Ok");
            //                return;
            //            }
            //        }
            //    }
            //}

            return true;
        }

        public bool Register(ContentPage page, string firstName, string surname, string email, string password)
        {          
            //    string tempCompanyId = entryCompanyId.Text;
            //    if (string.IsNullOrEmpty(tempCompanyId))
            //    {
            //        ClosePopup();
            //        await DisplayAlert("Warning", "Please Enter A Company Id", "Ok");
            //        return;
            //    }

            //    FileManager manager = new FileManager();
            //    List<Company> companies = await manager.GetAllCompanies();

            //    if (companies != null)
            //    {
            //        if (companies.Count > 0)
            //        {
            //            Company company = null;
            //            bool found = false;
            //            for (int i = 0; i < companies.Count; i++)
            //            {
            //                if (companies[i].CompanyID == tempCompanyId)
            //                {
            //                    found = true;
            //                    company = companies[i];
            //                    break;
            //                }
            //            }
            //            if (!found)
            //            {
            //                ClosePopup();
            //                companyIdError.IsVisible = true;
            //                await DisplayAlert("Warning", "Company Id Is Invalid", "Ok");
            //                return;
            //            }
            //            else
            //            {
            //                User user = new User() { FirstName = tempFirstName, Surname = tempSurname, Email = tempEmail, Password = txtPassword.Text, CompanyID = tempCompanyId, Privileges = 3, Accepted = false, Role = "" };
            //                await manager.AddNewUser(user);
            //                company.Employees.Add(user.Email);
            //                await manager.UpdateCompany(company.CompanyID, company);

            //                ClosePopup();
            //                await DisplayAlert("Success", "Successfully Created Account", "Ok");
            //                await Navigation.PopToRootAsync();
            //            }
            //        }
            //        else
            //        {
            //            ClosePopup();
            //            companyIdError.IsVisible = true;
            //            await DisplayAlert("Warning", "Company Id Is Invalid", "Ok");
            //            return;
            //        }
            //    }

            return false;
        }
    }
}

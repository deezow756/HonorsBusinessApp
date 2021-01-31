using System;
using System.Collections.Generic;
using System.Text;
using BusinessApp.Interfaces;
using BusinessApp.Models;

namespace BusinessApp.Controllers
{
    public class RegisterCompanyController : IRegisterCompanyController
    {
        private string[] ranChar = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public string GenerateCompanyID()
        {
            string randomString;
            bool match = false;
            do
            {
                match = false;
                randomString = GetRandomString(6);
                break;
                //FileManager file = new FileManager();
                //List<Company> companies = await file.GetAllCompanies();

                //if (companies != null)
                //{
                //    if (companies.Count > 0)
                //    {
                //        for (int i = 0; i < companies.Count; i++)
                //        {
                //            if (companies[i].CompanyID == randomString)
                //            {
                //                match = true;
                //                break;
                //            }
                //        }
                //    }
                //}
            } while (match);


            return randomString;
        }

        public bool Register(User user, string companyName, List<Role> roles)
        {
            //if (string.IsNullOrEmpty(temp))
            //{
            //    ClosePopup();
            //    await DisplayAlert("Warning", "Please Enter Your Company Name", "Ok");
            //    return;
            //}

            //user.Accepted = true;
            //user.CompanyID = txtCompanyId.Text;
            //user.Privileges = 3;

            //Company company = new Company() { Name = temp, CompanyID = txtCompanyId.Text, Roles = stringRoles, Employees = new List<string>() { user.Email } };

            //FileManager file = new FileManager();
            //await file.AddNewUser(user);
            //await file.AddNewCompany(company);
            return false;
        }

        private string GetRandomString(int size)
        {
            string randomString = "";
            for (int i = 0; i < size; i++)
            {
                Random random = new Random();
                randomString += ranChar[random.Next(0, ranChar.Length - 1)];
            }
            return randomString;
        }
    }
}

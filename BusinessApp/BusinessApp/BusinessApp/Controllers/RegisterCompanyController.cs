using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class RegisterCompanyController : IRegisterCompanyController
    {
        private string[] ranChar = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public bool CheckCompanyRegistationDetails(string companyName, List<Role> roles)
        {
            if(string.IsNullOrEmpty(companyName))
            {
                Dialog.Show("Warning", "Please Enter A CompanyName","Ok");
                return false;
            }

            if(roles.Count == 0)
            {
                Dialog.Show("Warning", "You Must Add At Least One Role", "Ok");
                return false;
            }

            return true;
        }

        public async Task<string> GenerateCompanyID()
        {
            string randomString;
            bool match = false;
            do
            {
                match = false;
                randomString = RandomGenerator.GenerateString(8);

                FirebaseHelper helper = new FirebaseHelper();
                List<Company> companies = await helper.GetAllCompanies();

                if (companies != null)
                {
                    if (companies.Count > 0)
                    {
                        for (int i = 0; i < companies.Count; i++)
                        {
                            if (companies[i].CompanyNumber == randomString)
                            {
                                match = true;
                                break;
                            }
                        }
                    }
                }
            } while (match);


            return randomString;
        }

        public async Task<bool> Register(User user, string companyName, string companyID, List<Role> roles)
        {
            if (user.CompanyIDs != null)
            { }
            else
            { user.CompanyIDs = new List<CompanyID>(); }
            user.CompanyIDs.Add(new CompanyID() { Access = 3, Approved = true, CompanyNumber = companyID, EmployeeNumber = RandomGenerator.GenerateNumber(6) , CurrentRole = new Role() { Name = "Owner"} });
            Company company = new Company() { Name = companyName, CompanyNumber = companyID, Roles = roles, AccountCreated = DateTime.Now, Employees = new List<User>() { user } };

            FirebaseHelper helper = new FirebaseHelper();
            User temp = await helper.GetUser(user.Email);
            if (temp != null)
            {
                await helper.UpdateUser(user.Email, user);
                await helper.AddNewCompany(company);
                return false;
            }
            else
            {
                await helper.AddNewUser(user);
                await helper.AddNewCompany(company);
                return true;
            }          
        }
    }
}

using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class MenuController : IMenuController
    {
        private List<Company> companies;

        public int CheckCompanyAccess(User user, string companyName)
        {
            Company company = companies.Find(a => a.Name == companyName);

            if (company != null)
            {
                CompanyID companyID = user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);
                return companyID.Access;
            }

            return 1;
        }

        public bool CheckCompanyApproved(User user, string companyName)
        {
            Company company = companies.Find(a => a.Name == companyName);

            if(company != null)
            {
                CompanyID companyID = user.CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);
                if (!companyID.Approved)
                    return false;

                return true;
            }

            return false;
        }

        public bool CheckCompanyIDs(List<CompanyID> companyIDs)
        {
            if (companyIDs != null)
            {
                if (companyIDs.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ConnectWithCompany(User user, string companyNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            companies = await helper.GetAllCompanies();
            Company company = companies.Find(a => a.CompanyNumber == companyNumber);

            if(company != null)
            {
                user.CompanyIDs.Add(new CompanyID() { Approved = false, CompanyNumber = company.CompanyNumber, EmployeeNumber = RandomGenerator.GenerateNumber(6) });
                company.Employees.Add(user);

                await helper.UpdateCompany(company);
                await helper.UpdateUser(user.Email, user);
               
                return true;
            }
            else
            {
                Dialog.Show("Warning", "Company Does not Exists With Company ID " + companyNumber, "Ok");
                return false;
            }    

        }

        public Task<User> GetUser(string email)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return helper.GetUser(email);
        }

        public async Task MenuSetup(Picker picker, List<CompanyID> companyIDs)
        {
            FirebaseHelper helper = new FirebaseHelper();
            companies = await helper.GetAllCompanies();

            picker.Items.Clear();
            for (int i = 0; i < companies.Count; i++)
            {
                for (int j = 0; j < companyIDs.Count; j++)
                {
                    if(companies[i].CompanyNumber.Equals(companyIDs[j].CompanyNumber))
                    {
                        picker.Items.Add(companies[i].Name);
                        break;
                    }
                }
            }
        }
    }
}

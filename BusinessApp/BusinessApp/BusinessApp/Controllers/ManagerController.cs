using BusinessApp.Models;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class ManagerController
    {
        public async Task<Company> GetCompany(string companyNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetCompany(companyNumber);
        }
        public List<User> GetEmployees(Company company)
        {
            List<User> users = new List<User>();

            for (int i = 0; i < company.Employees.Count; i++)
            {
                CompanyID companyID = company.Employees[i].CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);
                if (companyID.Approved)
                {
                    if(companyID.Access != 3)
                    {
                        users.Add(company.Employees[i]);
                    }
                }
            }

            return users;
        }
        public List<User> GetRequests(Company company)
        {
            List<User> users = new List<User>();

            for (int i = 0; i < company.Employees.Count; i++)
            {
                CompanyID companyID = company.Employees[i].CompanyIDs.Find(a => a.CompanyNumber == company.CompanyNumber);
                if (!companyID.Approved)
                {
                    users.Add(company.Employees[i]);
                }
            }

            return users;
        }
    }
}

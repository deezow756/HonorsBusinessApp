using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;

namespace BusinessApp.Controllers
{
    public class ProfileController : IProfileController
    {
        public bool CheckDetails(bool firstNameChanged, bool surnameChanged, bool emailChanged, string firstName, string surname, string email)
        {
            if(firstNameChanged)
            {
                if(string.IsNullOrEmpty(firstName))
                {
                    Dialog.Show("Warning", "First Name Is Invalid", "Ok");
                    return false;
                }
            }

            if (surnameChanged)
            {
                if (string.IsNullOrEmpty(surname))
                {
                    Dialog.Show("Warning", "Surname Is Invalid", "Ok");
                    return false;
                }
            }

            if (emailChanged)
            {
                if (string.IsNullOrEmpty(email))
                {
                    Dialog.Show("Warning", "Email Is Invalid", "Ok");
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> SaveChanges(User user, string firstName, string surname, string email)
        {
            string oldEmail = user.Email;
            user.FirstName = firstName.Trim();
            user.Surname = surname.Trim();
            user.Email = email.Trim();

            FirebaseHelper helper = new FirebaseHelper();
            if (user.CompanyIDs != null)
            {
                if (user.CompanyIDs.Count > 0)
                {                    
                    List<Company> companies = await helper.GetAllCompanies();

                    for (int i = 0; i < companies.Count; i++)
                    {
                        for (int j = 0; j < user.CompanyIDs.Count; j++)
                        {
                            if (companies[i].CompanyNumber == user.CompanyIDs[j].CompanyNumber)
                            {
                                companies[i].Employees.RemoveAll(a => a.AccountCreated == user.AccountCreated);
                                companies[i].Employees.Add(user);
                                await helper.UpdateCompany(companies[i]);
                                break;
                            }
                        }
                    }
                }
            }
            await helper.UpdateUser(oldEmail, user);

            return true;
        }

        public async Task<bool> ConnectWithCompany(User user, string companyNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            List<Company> companies = await helper.GetAllCompanies();
            Company company = companies.Find(a => a.CompanyNumber == companyNumber);

            if (company != null)
            {
                user.CompanyIDs.Add(new CompanyID() { Approved = false, CompanyNumber = company.CompanyNumber, Access = 0, CurrentRole = null, EmployeeNumber = RandomGenerator.GenerateNumber(6) });
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

        public async Task<List<Company>> GetCompanies(User user)
        {
            if (user.CompanyIDs != null)
            {
                if (user.CompanyIDs.Count > 0)
                {
                    List<Company> companies = new List<Company>();
                    FirebaseHelper helper = new FirebaseHelper();
                    List<Company> allCompanies = await helper.GetAllCompanies();

                    for (int i = 0; i < allCompanies.Count; i++)
                    {
                        for (int j = 0; j < user.CompanyIDs.Count; j++)
                        {
                            if (allCompanies[i].CompanyNumber == user.CompanyIDs[j].CompanyNumber)
                            {
                                companies.Add(allCompanies[i]);
                                break;
                            }
                        }
                    }

                    return companies;
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetUser(string email)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetUser(email);
        }

        public async Task<bool> AreYouSure(string title, string msg, string yesBtn, string noBtn)
        {
            return await Dialog.Show(title, msg, yesBtn, noBtn);
        }

        public async Task<bool> DisconnectWithCompany(User user, string companyNumber)
        {
            FirebaseHelper helper = new FirebaseHelper();
            List<Company> companies = await helper.GetAllCompanies();

            for (int i = 0; i < companies.Count; i++)
            {
                if(companies[i].CompanyNumber == companyNumber)
                {
                    for (int j = 0; j < companies[i].Employees.Count; j++)
                    {
                        if(companies[i].Employees[j].AccountCreated == user.AccountCreated)
                        {
                            companies[i].Employees.RemoveAt(j);
                            await helper.UpdateCompany(companies[i]);
                            break;
                        }
                    }
                }
            }

            user.CompanyIDs.RemoveAll(a => a.CompanyNumber == companyNumber);

            await helper.UpdateUser(user.Email, user);

            return true;
        }

        public bool CheckDisconnectEligible(User user, string companyNumber)
        {
            CompanyID companyID = user.CompanyIDs.Find(a => a.CompanyNumber == companyNumber);
            if (companyID.Access == 3)
            {
                Dialog.Show("Warning", "Cannot Disconnect From Company You Own", "Ok");
                return false;
            }
            else
                return true;
        }
    }
}

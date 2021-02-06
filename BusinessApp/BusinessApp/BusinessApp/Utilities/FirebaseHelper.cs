using BusinessApp.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Utilities
{
    public class FirebaseHelper
    {

        FirebaseClient firebase = new FirebaseClient("https://businessapp-2de82-default-rtdb.europe-west1.firebasedatabase.app/");

        #region UserEdit
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return (await firebase
                  .Child("Users")
                  .OnceAsync<User>()).Select(item => new User
                  {
                      Email = item.Object.Email,
                      Password = item.Object.Password,
                      FirstName = item.Object.FirstName,
                      Surname = item.Object.Surname,
                      CompanyIDs = item.Object.CompanyIDs,
                      AccountCreated = item.Object.AccountCreated
                  }).ToList();
            }
            catch(FirebaseException ex)
            {
                return null;
            }
            
        }

        public async Task AddNewUser(User user)
        {
            await firebase
              .Child("Users")
              .PostAsync(user);
        }

        public async Task<User> GetUser(string email)
        {
            var allUsers = await GetAllUsers();
            if (allUsers != null)
            {
                if (allUsers.Count > 0)
                {
                    await firebase
                      .Child("Users")
                      .OnceAsync<User>();
                    return allUsers.Where(a => a.Email == email).FirstOrDefault();
                }
                else { return null; }
            }
            return null;
        }

        public async Task UpdateUser(string email, User newInfo)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Email == email).FirstOrDefault();

            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(newInfo);
        }

        public async Task DeleteUser(User user)
        {
            var toDeletePerson = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Email == user.Email).FirstOrDefault();
            await firebase.Child("Users").Child(toDeletePerson.Key).DeleteAsync();

            List<Company> companies = await GetAllCompanies();

            for (int i = 0; i < user.CompanyIDs.Count; i++)
            {
                for (int j = 0; j < companies.Count; j++)
                {
                    Company company = null;
                    if(user.CompanyIDs[i].CompanyNumber == companies[j].CompanyNumber)
                    {
                        company = companies[j];
                    }

                    if (company != null)
                    {
                        company.Employees.RemoveAll(a => a.Email == user.Email);
                        await UpdateCompany(company);
                        break;
                    }
                }
            }           
        }

        #endregion

        #region CompanyEdit

        public async Task<List<Company>> GetAllCompanies()
        {
            return (await firebase
              .Child("Companies")
              .OnceAsync<Company>()).Select(item => new Company
              {
                  Name = item.Object.Name,
                  CompanyNumber = item.Object.CompanyNumber,
                  Employees = item.Object.Employees,
                  Roles = item.Object.Roles,
                  AccountCreated = item.Object.AccountCreated
              }).ToList();
        }
        public async Task AddNewCompany(Company company)
        {
            await firebase
              .Child("Companies")
              .PostAsync(company);
        }

        public async Task<Company> GetCompany(string companyId)
        {
            var allCompanies = await GetAllCompanies();
            await firebase
              .Child("Companies")
              .OnceAsync<Company>();
            return allCompanies.Where(a => a.CompanyNumber == companyId).FirstOrDefault();
        }

        public async Task UpdateCompany(Company company)
        {
            var toUpdateCompany = (await firebase
              .Child("Companies")
              .OnceAsync<Company>()).Where(a => a.Object.CompanyNumber == company.CompanyNumber).FirstOrDefault();

            await firebase
              .Child("Companies")
              .Child(toUpdateCompany.Key)
              .PutAsync(company);
        }

        public async Task DeleteCompany(string companyId)
        {
            var toDeletePerson = (await firebase
              .Child("Companies")
              .OnceAsync<Company>()).Where(a => a.Object.CompanyNumber == companyId).FirstOrDefault();
            await firebase.Child("Companies").Child(toDeletePerson.Key).DeleteAsync();
        }

        #endregion


    }
}

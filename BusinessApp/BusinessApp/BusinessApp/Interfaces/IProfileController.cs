using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;

namespace BusinessApp.Interfaces
{
    public interface IProfileController
    {
        Task<User> GetUser(string email);
        Task<List<Company>> GetCompanies(User user);
        Task<bool> ConnectWithCompany(User user, string companyNumber);
        bool CheckDetails(bool firstNameChanged, bool surnameChanged, bool emailChanged, string firstName, string surname, string email);
        Task<bool> SaveChanges(User user, string firstName, string surname, string email);
        Task<bool> AreYouSure(string title, string msg, string yesBtn, string noBtn);
        Task<bool> DisconnectWithCompany(User user, string companyNumber);
        bool CheckDisconnectEligible(User user, string companyNumber);
    }
}

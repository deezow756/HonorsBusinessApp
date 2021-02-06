using BusinessApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Interfaces
{
    public interface IMenuController
    {
        Task<User> GetUser(string email);
        bool CheckCompanyIDs(List<CompanyID> companyIDs);
        Task MenuSetup(Picker picker, List<CompanyID> companyIDs);
        bool CheckCompanyApproved(User user, string companyName);
        int CheckCompanyAccess(User user, string companyName);

        Task<bool> ConnectWithCompany(User user, string companyNumber);
    }
}

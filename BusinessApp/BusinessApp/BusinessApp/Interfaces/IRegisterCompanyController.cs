using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessApp.Models;
using Xamarin.Forms;

namespace BusinessApp.Interfaces
{
    public interface IRegisterCompanyController
    {
        bool CheckCompanyRegistationDetails(string companyName, List<Role> roles);
        Task<string> GenerateCompanyID();
        Task<bool> Register(User user, string companyName, string companyID, List<Role> roles);
    }
}

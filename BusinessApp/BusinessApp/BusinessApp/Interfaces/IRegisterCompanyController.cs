using System;
using System.Collections.Generic;
using System.Text;
using BusinessApp.Models;

namespace BusinessApp.Interfaces
{
    public interface IRegisterCompanyController
    {
        string GenerateCompanyID();
        bool Register(User user, string companyName, List<Role> roles);
    }
}

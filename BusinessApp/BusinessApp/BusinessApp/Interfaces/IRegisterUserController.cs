using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Interfaces
{
    public interface IRegisterUserController
    {
        Task<bool> CheckRegistrationDetails(string firstName, string surname, bool match, string email);
        Task<bool> Register(string firstName, string surname, string email, string password);
    }
}

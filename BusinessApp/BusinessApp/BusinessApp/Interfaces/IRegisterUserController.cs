using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Interfaces
{
    public interface IRegisterUserController
    {
        bool CheckRegistrationDetails(ContentPage page, string firstName, string surname, bool match, string email);
        bool Register(ContentPage page, string firstName, string surname, string email, string password);
    }
}

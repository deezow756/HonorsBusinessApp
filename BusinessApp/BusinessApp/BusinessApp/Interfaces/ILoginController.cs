using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Interfaces
{
    public interface ILoginController
    {
        bool CheckLoginValues(ContentPage page, string email, string password);
        bool Login(ContentPage page, string email, string password);
    }
}

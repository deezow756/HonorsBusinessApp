using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Interfaces
{
    public interface ILoginController
    {
        bool CheckLoginValues(string email, string password);
        Task<bool> LoginAsync(string email, string password);
    }
}

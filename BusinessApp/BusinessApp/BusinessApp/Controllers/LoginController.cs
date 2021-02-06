using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class LoginController : ILoginController
    {
        public bool CheckLoginValues(string email, string password)
        {            
            if (string.IsNullOrEmpty(email))
            {
                Dialog.Show("Warning", "Please Enter Your Email", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                Dialog.Show("Warning", "Please Enter Your Password", "Ok");
                return false;
            }

            return true;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            FirebaseHelper file = new FirebaseHelper();
            User user = await file.GetUser(email.ToLower());

            if (user != null)
            {
                if (user.Password == password)
                {
                    Dialog.Show("Success", "Successfully Signed In\nWelcome " + user.Name, "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new MenuView(user));
                }
                else
                {
                    Dialog.Show("Warning", "Incorrect Password", "Ok");
                    return false;
                }
            }
            else
            {
                Dialog.Show("Warning", "Please Enter A Valid Email", "Ok");
                return false;
            }

            return false;
        }
    }
}

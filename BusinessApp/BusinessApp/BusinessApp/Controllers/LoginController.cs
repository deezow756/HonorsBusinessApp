using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;
using BusinessApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace BusinessApp.Controllers
{
    public class LoginController
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

        public async Task<User> LoginAsync(string email, string password)
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Dialog.Show("No Internet", "Check Internet Connection And Try Again", "Ok");
                return null;
            }
            FirebaseHelper file = new FirebaseHelper();
            password = Security.EncryptPassword(password);
            User user = await file.GetUser(email.ToLower());

            email = email.Trim();
            password = password.Trim();
            if (user != null)
            {
                if (user.Password == password)
                {
                    await Dialog.Show("Success", "Successfully Signed In\nWelcome " + user.Name, "Ok");
                    return user;
                }
                else
                {
                    await Dialog.Show("Warning", "Incorrect Password", "Ok");
                    return null;
                }
            }
            else
            {
                await Dialog.Show("Warning", "Please Enter A Valid Email", "Ok");
                return null;
            }
        }

        public async void DisplayHelp()
        {
            await Dialog.Show("Help", "To login please enter your email and password or if you do not yet have an account click the register button", "Ok");
        }
    }
}

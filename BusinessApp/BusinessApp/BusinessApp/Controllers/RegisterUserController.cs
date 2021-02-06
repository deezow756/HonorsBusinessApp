using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessApp.Interfaces;
using BusinessApp.Models;
using BusinessApp.Utilities;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class RegisterUserController : IRegisterUserController
    {
        public async Task<bool> CheckRegistrationDetails(string firstName, string surname, bool match, string email)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                Dialog.Show("Warning", "Please Enter A First Name", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(surname))
            {
                Dialog.Show("Warning", "Please Enter A Surname", "Ok");
                return false;
            }

            if (match == false)
            {
                Dialog.Show("Warning", "Please Enter Valid Password", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                Dialog.Show("Warning", "Please Enter An Email", "Ok");
                return false;
            }

            FirebaseHelper helper = new FirebaseHelper();
            List<User> users = await helper.GetAllUsers();
            if (users != null)
            {
                if (users.Count > 0)
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (users[i].Email == email)
                        {
                            Dialog.Show("Warning", "Email Already Used", "Ok");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public async Task<bool> Register(string firstName, string surname, string email, string password)
        {
            FirebaseHelper helper = new FirebaseHelper();
            User user = new User(firstName, surname, email, password);
            await helper.AddNewUser(user);
            Dialog.Show("Success", "Successfully Created Account", "Ok");

            return true;
        }
    }
}

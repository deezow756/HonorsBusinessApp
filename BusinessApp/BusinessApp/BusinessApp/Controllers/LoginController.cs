using BusinessApp.Interfaces;
using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Controllers
{
    public class LoginController : ILoginController
    {
        public bool CheckLoginValues(ContentPage page, string email, string password)
        {            
            if (string.IsNullOrEmpty(email))
            {
                Dialog.Show(page, "Warning", "Please Enter Your Email", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                Dialog.Show(page, "Warning", "Please Enter Your Password", "Ok");
                return false;
            }

            return true;
        }

        public bool Login(ContentPage page, string email, string password)
        {
            //FileManager file = new FileManager();
            //User user = await file.GetUser(tempEmail.ToLower());

            //if (user != null)
            //{
            //    if (user.Password == tempPassword)
            //    {
            //        ClosePopup();
            //        txtEmail.Text = "";
            //        txtPassword.Text = "";
            //        await DisplayAlert("Success", "Successfully Signed In\nWelcome " + user.Name, "Ok");
            //        await Navigation.PushAsync(new Menu(user));
            //    }
            //    else
            //    {
            //        ClosePopup();
            //        await DisplayAlert("Warning", "Incorrect Password", "Ok");
            //        return;
            //    }
            //}
            //else
            //{
            //    ClosePopup();
            //    await DisplayAlert("Warning", "Please Enter A Valid Email", "Ok");
            //    return;
            //}

            return false;
        }

    }
}

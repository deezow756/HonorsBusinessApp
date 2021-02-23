using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusinessApp.Utilities
{
    public class Dialog
    {
        async public static Task Show(string title, string msg, string btn)
        {
            await Application.Current.MainPage.DisplayAlert(title, msg, btn);
        }

        async public static Task<bool> Show(string title, string msg, string yesBtn, string noBtn)
        {
            return await Application.Current.MainPage.DisplayAlert(title, msg, yesBtn, noBtn);
        }
    }
}

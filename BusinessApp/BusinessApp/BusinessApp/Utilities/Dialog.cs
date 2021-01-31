using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BusinessApp.Utilities
{
    public class Dialog
    {
        async public static void Show(ContentPage page, string title, string msg, string btn)
        {
            await page.DisplayAlert("Warning", "Please Enter Your Email", "Ok");
        }
    }
}

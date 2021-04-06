using BusinessApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class NoInternetController
    {
        public async Task<bool> AreYouSure()
        {
            return await Dialog.Show("Warning", "Are You Sure You Want To Return To The Login Page?", "Yes", "No");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BusinessApp.Utilities
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup : ContentView
    {
        public LoadingPopup()
        {
            InitializeComponent();
            Animate();
        }

        async void Animate()
        {
            do
            {
                await imageLoading.RotateTo(360, 2000);
                imageLoading.Rotation = 0;
            } while (true);
        }
    }
}
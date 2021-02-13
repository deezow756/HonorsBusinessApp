using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.Res;
using BusinessApp.Themes;
using Xamarin.Forms;
using Android.Support.V7.App;

namespace BusinessApp.Droid
{
    [Activity(Label = "BusinessApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            MessagingCenter.Subscribe<object, ThemeType>(App.Current, "ChangeTheme", async (sender, arg) =>
            {
                SetAppTheme(arg);
            });
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            ThemeHelper.ChangeTheme();
        }

        void SetAppTheme(ThemeType arg)
        {
            if (arg == ThemeType.Dark)
                Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
            else
                Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
        }

        

    }
}
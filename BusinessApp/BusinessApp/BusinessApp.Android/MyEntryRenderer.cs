using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BusinessApp.Droid;
using BusinessApp.Themes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BusinessApp.Utilities;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace BusinessApp.Droid
{
    public class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null || e.NewElement == null) return;

            SetColour();
        }

        private void SetColour()
        {
            if (ThemeHelper.CurrentTheme == ThemeType.Dark)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Gray);
                else
                    Control.Background.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcAtop);
            }
            else
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Gray);
                else
                    Control.Background.SetColorFilter(Android.Graphics.Color.Black, PorterDuff.Mode.SrcAtop);
            }
        }
    }
}
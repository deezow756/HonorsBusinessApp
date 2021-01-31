using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessApp.iOS;
using BusinessApp.Themes;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using BusinessApp.Utilities;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace BusinessApp.iOS
{
    public class MyEntryRenderer : EntryRenderer
    {
        private CALayer _line;

        public MyEntryRenderer() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);           

            if (Control == null || e.NewElement == null)
                return;
            SetColour();
        }

        private void SetColour()
        {
            _line = null;
            Control.BorderStyle = UITextBorderStyle.None;

            if (ThemeHelper.CurrentTheme == ThemeType.Dark)
            {
                _line = new CALayer
                {
                    BorderColor = UIColor.FromRGB(255, 255, 255).CGColor,
                    BackgroundColor = UIColor.FromRGB(0, 0, 0).CGColor,
                    Frame = new CGRect(0, Frame.Height / 2, Frame.Width * 2, 1f)
                };
            }
            else
            {
                _line = new CALayer
                {
                    BorderColor = UIColor.FromRGB(0, 0, 0).CGColor,
                    BackgroundColor = UIColor.FromRGB(255, 255, 255).CGColor,
                    Frame = new CGRect(0, Frame.Height / 2, Frame.Width * 2, 1f)
                };
            }

            Control.Layer.AddSublayer(_line);
        }
    }
}
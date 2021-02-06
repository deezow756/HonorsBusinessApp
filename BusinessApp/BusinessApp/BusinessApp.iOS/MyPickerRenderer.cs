using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessApp.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(MyPickerRenderer))]
namespace BusinessApp.iOS
{
    public class MyPickerRenderer: PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
                return;
            Control.Layer.BorderWidth = 1;
            Control.Layer.BorderColor = Color.Gray.ToCGColor();
        }
    }
}
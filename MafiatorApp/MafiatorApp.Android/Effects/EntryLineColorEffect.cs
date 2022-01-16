using Android.Widget;
using MafiatorApp.Behaviors;
using MafiatorApp.Droid.Effects;
using System;
using System.ComponentModel;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MafiatorApp")]
[assembly: ExportEffect(typeof(EntryLineColorEffect), "EntryLineColorEffect")]
namespace MafiatorApp.Droid.Effects
{
    public class EntryLineColorEffect:PlatformEffect
    {
        private EditText control;

        protected override void OnAttached()
        {
            try
            {
                control = Control as EditText;
                UpdateLineColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            control = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == LineColorBehavior.LineColorProperty.PropertyName)
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            try
            {
                control?.Background.SetColorFilter(new PorterDuffColorFilter(LineColorBehavior.GetLineColor(Element).ToAndroid(),PorterDuff.Mode.SrcAtop));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
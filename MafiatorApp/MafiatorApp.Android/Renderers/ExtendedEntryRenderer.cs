using Android.Content;
using Android.Content.Res;
using MafiatorApp.Droid.Renderers;
using MafiatorApp.UserControls;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using Path = System.IO.Path;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace MafiatorApp.Droid.Renderers
{
    public class ExtendedEntryRenderer : MaterialEntryRenderer
    {
        public ExtendedEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            Control.ErrorEnabled = true;
            Control.SetErrorTextColor(ColorStateList.ValueOf(Color.ParseColor("#FF204F")));
            Control.BoxBackgroundColor = Application.Current.RequestedTheme == OSAppTheme.Light ? ((Xamarin.Forms.Color)Application.Current.Resources["Day1"]).ToAndroid() : ((Xamarin.Forms.Color)Application.Current.Resources["Night1"]).ToAndroid();
            Control.BoxStrokeColor = Color.Red;
            Control.SetBoxCornerRadii(36,36,36,36);
            SetIcon((ExtendedEntry)Element);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColorToApply)))
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            var errors = ((ExtendedEntry) Element).Errors;
            Control.Error = errors.Any() ? errors[0]:"";
        }
        private void SetIcon(ExtendedEntry view)
        {
            if (!string.IsNullOrEmpty(view.Icon))
            {
                try
                {
                    var context = Android.App.Application.Context;
                    var resId = context.Resources.GetIdentifier(Path.GetFileNameWithoutExtension(view.Icon), "drawable", context.PackageName);
                    if(resId != 0)
                        Control.EditText?.SetCompoundDrawablesWithIntrinsicBounds(0, 0, resId, 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Control.EditText?.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
            }
        }
    }
}
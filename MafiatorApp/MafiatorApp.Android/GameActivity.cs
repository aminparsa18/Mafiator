using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MafiatorApp.Droid
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity 
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            if (Intent.Data != null)
            {
                var uri = new Uri(Intent.Data?.ToString());
                LoadApplication(new App(uri));
            }
            else
            {
                LoadApplication(new App(new Uri("about:blank")));
            }
        }
    }
}
using Android.App;
using Android.OS;

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
         
        }
    }
}
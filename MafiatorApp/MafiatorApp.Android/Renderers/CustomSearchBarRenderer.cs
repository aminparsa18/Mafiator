using Android.Content;
using Android.Widget;
using MafiatorApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace MafiatorApp.Droid.Renderers
{
    public class CustomSearchBarRenderer:SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var searchView = Control;
                searchView.Iconified = true;
                searchView.SetIconifiedByDefault(false);
                // (Resource.Id.search_mag_icon); is wrong / Xammie bug
                var searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                var icon = searchView.FindViewById(searchIconId);
                (icon as ImageView)?.SetImageResource(Resource.Drawable.search);
                (icon as ImageView).LayoutParameters.Width = 48;
            }
        }
    }
}
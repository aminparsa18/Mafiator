using Android.Content;
using AndroidX.Core.Content;
using MafiatorApp.Droid.Renderers;
using MafiatorApp.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(WaitingProgressBar), typeof(WaitingProgressBarRenderer))]
namespace MafiatorApp.Droid.Renderers
{
    public class WaitingProgressBarRenderer : ProgressBarRenderer
    {
        public WaitingProgressBarRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.ProgressDrawable = ContextCompat.GetDrawable(Context, Resource.Drawable.custom_progress_bar);
                Control.SetMinimumHeight(240);
            }
        }
    }
}
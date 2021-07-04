using Android.Content;
using Android.Widget;
using Android.Gms.Ads;
using Android.Util;
using MafiatorApp.Droid.Renderers;
using MafiatorApp.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

[assembly: ExportRenderer(typeof(AdControlView), typeof(AdViewRenderer))]
namespace MafiatorApp.Droid.Renderers
{
    public class AdViewRenderer : ViewRenderer<AdControlView, AdView>
    {
        string adUnitId = string.Empty;
        //Note you may want to adjust this, see further down.
        readonly AdSize adSize = GetFullWidthAdaptiveSize();
        AdView adView;
        public AdViewRenderer(Context context) : base(context)
        {
        }
        AdView CreateNativeAdControl()
        {
            if (adView != null)
                return adView;

            // This is a string in the Resources/values/strings.xml that I added or you can modify it here. This comes from admob and contains a / in it
            adUnitId = "ca-app-pub-3940256099942544/6300978111";// "ca-app-pub-5204679523132638/6212946482";
            adView = new AdView(Context) {AdSize = adSize, AdUnitId = adUnitId};

            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            adView.LayoutParameters = adParams;
            adView.LoadAd(new AdRequest
                    .Builder()
                .Build());
            return adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);
            if (Control != null) return;
            CreateNativeAdControl();
            SetNativeControl(adView);
        }
        private static AdSize GetFullWidthAdaptiveSize()
        {
            var display = Platform.CurrentActivity.WindowManager.DefaultDisplay;
            var outMetrics = new DisplayMetrics();
            display.GetRealMetrics(outMetrics);
            float widthPixels = outMetrics.WidthPixels;
            var density = outMetrics.Density;
            var adWidth = (int)(widthPixels / density);
            return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSize(Platform.CurrentActivity, adWidth);
        }
    }
}
using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Runtime;
using Android.OS;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms;
using Android.Content;
using Android.Gms.Ads;
using Android.Views;
using AndroidX.Core.App;
using MafiatorApp.Droid.Recorder;
using MafiatorApp.Services;
using MediaManager;
//using Xamarin.Auth;

namespace MafiatorApp.Droid
{
    [Activity(Label = "Mafiator", Icon = "@drawable/logo", Theme = "@style/BaseTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(
        new[] {Intent.ActionView},
        Categories = new[] {Intent.CategoryDefault, Intent.CategoryBrowsable},
        DataSchemes = new[] {"app"},
        DataHost = "invite.mftor")]
    [IntentFilter(
        new[] {Intent.ActionView},
        Categories = new[] {Intent.CategoryDefault, Intent.CategoryBrowsable},
        DataSchemes = new[] {"app"},
        DataHost = "callback.mftor")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            base.OnCreate(savedInstanceState);
            CachedImageRenderer.Init(true);
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            GoogleVisionBarCodeScanner.Droid.RendererInitializer.Init();
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            DependencyService.RegisterSingleton<IAudioRecorder>(new AudioRecorder());
            DependencyService.RegisterSingleton<IAudioStream>(new AudioStream(44100, 48));
            CrossMediaManager.Current.Init(this);
            MobileAds.Initialize(this);
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

        public override Android.Content.Res.Resources Resources
        {
            get
            {
                var res = base.Resources;
                var config = new Configuration();
                config.SetToDefaults();
                //if (Build.VERSION.SdkInt >= BuildVersionCodes.NMr1)
                //  return CreateConfigurationContext(config)?.Resources;
                res?.UpdateConfiguration(config, res.DisplayMetrics);
                return res;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //   ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
         
        }

    }
}
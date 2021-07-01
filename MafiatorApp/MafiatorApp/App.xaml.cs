using System;
using MafiatorApp.Cache;
using MafiatorApp.UserControls;
using MafiatorApp.UserControls.ShimmerLayout;
using MafiatorApp.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;

namespace MafiatorApp
{
    public partial class App : Application
    {
        public App(Uri uri)
        {
            Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();
            // CultureInfo.CurrentUICulture = new CultureInfo("fa-IR", false);
            // CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fa-IR", false);
           // CrossMTAdmob.Current.UseRestrictedDataProcessing = true;
           // CrossMTAdmob.Current.AdsId = "ca-app-pub-3940256099942544/6300978111";
            Barrel.ApplicationId = "Mafiator";
            Barrel.EncryptionKey = "NJR*fgpB5a";
            VersionTracking.Track();
            AppCenter.Start("android=4a514da9-9b2f-458a-92a9-62f9e7abfffb;" +
                            "uwp={Your UWP App secret here};" +
                            "ios={Your iOS App secret here}",
                typeof(Analytics), typeof(Crashes));
            ShimmerLayout.Init(DeviceDisplay.MainDisplayInfo.Density);
            //if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6)
              // UserAppTheme = OSAppTheme.Dark;
            //else
            //    UserAppTheme = OSAppTheme.Light;

            MainPage = new TransitionNavigationPage(new SplashScreenView(uri));
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
           CrossSimpleAudioPlayer.Current.Pause();
        }

        protected override void OnResume()
        {
            if(CrossSimpleAudioPlayer.Current.CanSeek)
                CrossSimpleAudioPlayer.Current.Play();

        }

    }
}
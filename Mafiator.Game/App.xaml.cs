using Mafiator.Common.Client.Cache;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.MauiMTAdmob;
using System.Globalization;

namespace Mafiator.Game;

public partial class App : Application
{
    public Uri AppUri { get; }

    public App()
    {
        InitializeComponent();

        Barrel.ApplicationId = "Mafiator";
        Barrel.EncryptionKey = "NJR*fgpB5a";
        if (Barrel.Current.Exists("Culture"))
        {
            if (Barrel.Current.Get<string>("Culture") == "RU")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU", false);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU", false);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
            }
        }

        VersionTracking.Track();
        AppCenter.Start("android=4a514da9-9b2f-458a-92a9-62f9e7abfffb;" +
                        "uwp={Your UWP App secret here};" +
                        "ios={Your iOS App secret here}",
            typeof(Analytics), typeof(Crashes));

        CrossMauiMTAdmob.Current.UserPersonalizedAds = true;
        CrossMauiMTAdmob.Current.AdsId = "xxxxxxxxxxxxxxxx";

        // ShimmerLayout.Init(DeviceDisplay.MainDisplayInfo.Density);
        //if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6)
        // UserAppTheme = OSAppTheme.Dark;
        //else
        //    UserAppTheme = OSAppTheme.Light;

        MainPage = new AppShell();// NavigationPage(new SplashScreenView(uri));
    }

    public static Color GetColorFromResource(string key)
    {
        var color = Current.Resources.MergedDictionaries.FirstOrDefault()[key];
        return color != null ? (Color)color : throw new ArgumentNullException($"color resource with key : {key} not found");
    }
}
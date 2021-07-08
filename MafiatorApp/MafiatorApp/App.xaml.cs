using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using FFImageLoading;
using MafiatorApp.Cache;
using MafiatorApp.Models;
using MafiatorApp.Resources.Texts;
using MafiatorApp.UserControls;
using MafiatorApp.UserControls.ShimmerLayout;
using MafiatorApp.Views;
using MessagePack;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.SimpleAudioPlayer;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
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

            LocalizationResourceManager.Current.PropertyChanged += (_, _) =>
                AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

            Barrel.ApplicationId = "Mafiator";
            Barrel.EncryptionKey = "NJR*fgpB5a";
            InitBarrel();
            if (Barrel.Current.Exists("Culture"))
            {
                if (Barrel.Current.Get<string>("Culture") == "RU")
                {
                    LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("ru-RU", false);
                }
                else
                {
                    LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en-US", false);
                }
            }

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

            MainPage = new TransitionNavigationPage(new HomeView());
        }

        private async void InitBarrel()
        {
            if (!Barrel.Current.Exists("Countries"))
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MafiatorApp.countries.json");
                var countries = await JsonSerializer.DeserializeAsync<List<Country>>(stream);
                Barrel.Current.Add("Countries", countries, TimeSpan.MaxValue);
            }
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
            if (CrossSimpleAudioPlayer.Current.CanSeek)
                CrossSimpleAudioPlayer.Current.Play();
        }
    }
}
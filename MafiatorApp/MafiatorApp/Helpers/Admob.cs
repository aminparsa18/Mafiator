using MarcTron.Plugin;
using System;

namespace MafiatorApp.Helpers
{
    public static class Admob
    {
        public static void Load()
        {
            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
            CrossMTAdmob.Current.OnInterstitialLoaded += InterstitialLoaded;
        }

        private static void InterstitialLoaded(object sender, EventArgs e)
        {
            CrossMTAdmob.Current.ShowInterstitial();
            CrossMTAdmob.Current.OnInterstitialLoaded -= InterstitialLoaded;
        }
    }
}
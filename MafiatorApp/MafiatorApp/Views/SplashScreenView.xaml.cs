using System;
using MafiatorApp.Cache;
using MafiatorApp.ViewModels;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenView : ContentPage
    {
        private Uri uri;
        public SplashScreenView(Uri uri)
        {
            this.uri = uri;
           NavigationPage.SetHasNavigationBar(this,false);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (CrossSimpleAudioPlayer.Current.Load("shoot.mp3") && (!Barrel.Current.Exists("PlaySound") || Barrel.Current.Get<bool>("PlaySound")))
                    CrossSimpleAudioPlayer.Current.Play();
                await Logo.FadeTo(1, 600, Easing.Linear);
                await Logo.RelRotateTo(360, 400, Easing.Linear);
                await AppName.FadeTo(1, 800, Easing.Linear);
                await ((SplashScreenViewModel) BindingContext).Navigate(uri);
               // Navigation.RemovePage(this);

            });
          
          
        }
    }
}
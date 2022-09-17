using Mafiator.Common.Client.Cache;
using MafiatorApp.ViewModels;
using MediaManager;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenView : ContentPage
    {
        public SplashScreenView()
        {
            NavigationPage.SetHasNavigationBar(this,false);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!Barrel.Current.Exists("PlaySound") || Barrel.Current.Get<bool>("PlaySound"))
                    await CrossMediaManager.Current.PlayFromAssembly("shoot.mp3", Assembly.GetExecutingAssembly());
                await Logo.FadeTo(1, 600, Easing.Linear);
                await Logo.RelRotateTo(360, 400, Easing.Linear);
                await AppName.FadeTo(1, 800, Easing.Linear);
                await ((SplashScreenViewModel)this.BindingContext).Navigate();
            });
        }
    }
}
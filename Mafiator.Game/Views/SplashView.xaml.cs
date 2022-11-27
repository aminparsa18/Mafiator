using Mafiator.Common.Client.Cache;
using Mafiator.Game.ViewModels;
using Plugin.Maui.Audio;

namespace Mafiator.Game.Views;

public partial class SplashView : ContentPage
{
    private readonly IAudioManager _audioManager;

    public SplashView(IAudioManager audioManager, SplashScreenViewModel viewModel)
    {
        InitializeComponent();
        _audioManager = audioManager;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (!Barrel.Current.Exists("PlaySound") || Barrel.Current.Get<bool>("PlaySound"))
            {
                var audioPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("shoot.mp3"));
                audioPlayer.Play();
            }
            await Logo.FadeTo(1, 600, Easing.Linear);
            await Logo.RelRotateTo(360, 400, Easing.Linear);
            await AppName.FadeTo(1, 800, Easing.Linear);
            await (BindingContext as SplashScreenViewModel).Navigate();
        });
    }
}
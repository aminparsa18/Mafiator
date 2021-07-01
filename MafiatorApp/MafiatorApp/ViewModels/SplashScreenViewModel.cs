using System;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.ViewModels.Base;
using Plugin.SimpleAudioPlayer;

namespace MafiatorApp.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        public async Task Navigate(Uri uri)
        {
            if (Barrel.Current.Exists("Token"))
            {
                if (uri.Segments.Length == 3)
                {
                    var path = uri.Segments[1];
                    if (path.StartsWith("room"))
                        await NavigationService.NavigateToAsync<MyRoomViewModel>(Ulid.Parse(uri.Segments[2]));
                    else if (path.StartsWith("game"))
                        await NavigationService.NavigateToAsync<WaitingGameViewModel>(Ulid.Parse(uri.Segments[2]));
                }
                else
                    await NavigationService.NavigateToAsync<HomeViewModel>();
            }
            else
                await NavigationService.NavigateToAsync<LoginViewModel>();

            if ((!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic")) && CrossSimpleAudioPlayer.Current.Load("mafia1.mp3"))
                CrossSimpleAudioPlayer.Current.Play();
        }
    }
}
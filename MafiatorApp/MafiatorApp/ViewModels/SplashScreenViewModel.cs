using System;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Plugin.SimpleAudioPlayer;

namespace MafiatorApp.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly ISubscriber<ChangeLanguageEvent> _subscriber;
        private readonly IDisposable _disposable;

        public SplashScreenViewModel(ISubscriber<ChangeLanguageEvent> subscriber)
        {
            _subscriber = subscriber;
            var bag = DisposableBag.CreateBuilder();
            _subscriber.Subscribe(async _ => { await Navigate(new Uri("about:blank")); }).AddTo(bag);
            _disposable = bag.Build();
        }

        public async Task Navigate(Uri uri)
        {
            if (!Barrel.Current.Exists("Culture"))
            {
                await NavigationService.NavigateToPopupAsync<LanguagesViewModel>();
                return;
            }

            if (Barrel.Current.Exists("Token"))
            {
                if (uri.Segments.Length == 3)
                {
                    var path = uri.Segments[1];
                    if (path.StartsWith("room"))
                     await NavigationService.NavigateToAsync<RoomDetailViewModel>(Ulid.Parse(uri.Segments[2]));
                    else if (path.StartsWith("game"))
                       await NavigationService.NavigateToAsync<WaitingGameViewModel>(Ulid.Parse(uri.Segments[2]));
                }
                else
                    await NavigationService.NavigateToAsync<HomeViewModel>();
            }
            else
                await NavigationService.NavigateToAsync<LoginViewModel>();

            if ((!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic")) &&
                CrossSimpleAudioPlayer.Current.Load("mafia1.mp3"))
                CrossSimpleAudioPlayer.Current.Play();
            _disposable.Dispose();
        }
    }
}
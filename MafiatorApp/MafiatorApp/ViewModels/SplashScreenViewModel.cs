using Mafiator.Common.Client.Cache;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MediaManager;
using MediaManager.Playback;
using MessagePipe;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MafiatorApp.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly IAsyncSubscriber<ChangeLanguageEvent> _subscriber;
        private readonly IDisposable _disposable;

        public SplashScreenViewModel(IAsyncSubscriber<ChangeLanguageEvent> subscriber)
        {
            _subscriber = subscriber;
            var bag = DisposableBag.CreateBuilder();
            _subscriber.Subscribe(async (_,_) =>
            {
                await Navigate(new Uri("about:blank"));
            }).AddTo(bag);
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
                       await NavigationService.NavigateToAsync<RoomDetailViewModel>(Guid.Parse(uri.Segments[2]));
                    else if (path.StartsWith("game"))
                       await NavigationService.NavigateToAsync<WaitingGameViewModel>(Guid.Parse(uri.Segments[2]));
                }
                else
                    await NavigationService.NavigateToAsync<HomeViewModel>();
            }
            else
                await NavigationService.NavigateToAsync<LoginViewModel>();

            if (!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic"))
                await CrossMediaManager.Current.PlayFromAssembly("mafia1.mp3", Assembly.GetExecutingAssembly());
            CrossMediaManager.Current.Notification.Enabled = false;
            CrossMediaManager.Current.RepeatMode = RepeatMode.All;
            _disposable.Dispose();
        }
    }
}
using Mafiator.Common.Client.Cache;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using System.Diagnostics;

namespace Mafiator.Game.ViewModels;

public class SplashViewModel : ViewModelBase
{
    private readonly IAsyncSubscriber<ChangeLanguageEvent> _subscriber;
    private readonly IDisposable _disposable;

    public SplashViewModel(INavigationService navigationService, IToastService toastService, 
        IAsyncSubscriber<ChangeLanguageEvent> subscriber) : base(navigationService, toastService)
    {
        _subscriber = subscriber;
        var bag = DisposableBag.CreateBuilder();
        _subscriber.Subscribe(async (_, _) => await Navigate()).AddTo(bag);
        _disposable = bag.Build();
    }

    public async Task Navigate()
    {
        if (!Barrel.Current.Exists("Culture"))
        {
            await _navigationService.NavigateToPopupAsync<LanguagesViewModel>();
            return;
        }

        if (Barrel.Current.Exists("Token"))
        {
            _disposable.Dispose();
            var segments = (Application.Current as App).AppUri?.Segments;
            if (segments?.Length == 3)
            {
                var path = segments[1];
                if (path.StartsWith("room"))
                    await _navigationService.NavigateToAsync($"{nameof(RoomDetailViewModel)}?roomId={Guid.Parse(segments[2])}");
                else if (path.StartsWith("game"))
                    await _navigationService.NavigateToAsync($"{nameof(WaitingGameViewModel)}?gameId={Guid.Parse(segments[2])}");
            }
            else
                await _navigationService.NavigateToAsync(nameof(HomeViewModel));
        }
        else
        {
            await _navigationService.NavigateToAsync(nameof(LoginViewModel));
        }

        //if (!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic"))
        //    await CrossMediaManager.Current.PlayFromAssembly("mafia1.mp3", Assembly.GetExecutingAssembly());
        //CrossMediaManager.Current.Notification.Enabled = false;
        //CrossMediaManager.Current.RepeatMode = RepeatMode.All;
    }
}
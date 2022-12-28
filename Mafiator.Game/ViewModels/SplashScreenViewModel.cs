using Mafiator.Common.Client.Cache;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Mafiator.Game.ViewModels;

public class SplashScreenViewModel : ViewModelBase
{
    private readonly IAsyncSubscriber<ChangeLanguageEvent> _subscriber;
    private readonly IDisposable _disposable;

    public SplashScreenViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
        IAsyncSubscriber<ChangeLanguageEvent> subscriber) : base(navigationService, localizer, toastService)
    {
        _subscriber = subscriber;
        var bag = DisposableBag.CreateBuilder();
        _subscriber.Subscribe(async (_, _) =>
        {
            await Navigate();
        }).AddTo(bag);
        _disposable = bag.Build();
    }

    public async Task Navigate()
    {
        if (!Barrel.Current.Exists("Culture"))
        {
            await _navigationService.NavigateToPopupAsync<LanguagesViewModel>();
            return;
        }

        if (!Barrel.Current.Exists("Token"))
        {
            var segments = (Application.Current as App).AppUri?.Segments;
            if (segments.Length == 3)
            {
                var path = segments[1];
                if (path.StartsWith("room"))
                    await _navigationService.NavigateToAsync<RoomDetailViewModel>(Guid.Parse(segments[2]), replace: true);
                else if (path.StartsWith("game"))
                    await _navigationService.NavigateToAsync<WaitingGameViewModel>(Guid.Parse(segments[2]), replace: true);
            }
            else
                await _navigationService.NavigateToAsync<HomeViewModel>(true);
        }
        else
            await _navigationService.NavigateToAsync<LoginViewModel>(true);

        //if (!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic"))
        //    await CrossMediaManager.Current.PlayFromAssembly("mafia1.mp3", Assembly.GetExecutingAssembly());
        //CrossMediaManager.Current.Notification.Enabled = false;
        //CrossMediaManager.Current.RepeatMode = RepeatMode.All;
        _disposable.Dispose();
    }
}
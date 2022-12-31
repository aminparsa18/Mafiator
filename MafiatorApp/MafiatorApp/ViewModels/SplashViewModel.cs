using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;

namespace MafiatorApp.ViewModels
{
    public class SplashViewModel : ViewModelBase
    {
        private readonly IAsyncSubscriber<ChangeLanguageEvent> _subscriber;
        private readonly IDisposable _disposable;

        public SplashViewModel(IAsyncSubscriber<ChangeLanguageEvent> subscriber)
        {
            _subscriber = subscriber;
            var bag = DisposableBag.CreateBuilder();
            _subscriber.Subscribe(async (_,_) =>
            {
                await Navigate();
            }).AddTo(bag);
            _disposable = bag.Build();
        }

        public async Task Navigate()
        {
            if (!Barrel.Current.Exists("Culture"))
            {
                await NavigationService.NavigateToPopupAsync<LanguagesViewModel>();
                return;
            }

            if (!Barrel.Current.Exists("Token"))
            {
                var segments = (Application.Current as App).AppUri.Segments;
                if (segments.Length == 3)
                {
                    var path = segments[1];
                    if (path.StartsWith("room"))
                       await NavigationService.NavigateToAsync<RoomDetailViewModel>(Guid.Parse(segments[2]), replace:true);
                    else if (path.StartsWith("game"))
                       await NavigationService.NavigateToAsync<WaitingGameViewModel>(Guid.Parse(segments[2]), replace: true);
                }
                else
                    await NavigationService.NavigateToAsync<HomeViewModel>(true);
            }
            else
                await NavigationService.NavigateToAsync<LoginViewModel>(true);

            if (!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic"))
                await CrossMediaManager.Current.PlayFromAssembly("mafia1.mp3", Assembly.GetExecutingAssembly());
            CrossMediaManager.Current.Notification.Enabled = false;
            CrossMediaManager.Current.RepeatMode = RepeatMode.All;
            _disposable.Dispose();
        }
    }
}
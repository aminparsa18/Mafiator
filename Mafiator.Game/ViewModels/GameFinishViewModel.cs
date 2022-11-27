using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
{
    public class GameFinishViewModel : ViewModelBase
    {
        private string winner;

        public string Winner
        {
            get => winner;
            set => SetProperty(ref winner, value);
        }

        private int _timer;
        private double progressTimer;

        public double ProgressTimer
        {
            get => progressTimer;
            set => SetProperty(ref progressTimer, value);
        }

        public GameFinishViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService)
            : base(navigationService, localizer, toastService)
        {
            Dispatcher.GetForCurrentThread().StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                _timer += 100;
                ProgressTimer = 100 * (double)_timer / 8000;
                if (_timer != 8000)
                    return true;
                navigationService.NavigateToAsync<HomeViewModel>(true);
                return false;

            });
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string winner)
                Winner = winner + " Wins";

            return base.InitializeAsync(navigationData);
        }
    }
}
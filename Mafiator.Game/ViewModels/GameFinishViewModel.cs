using CommunityToolkit.Mvvm.ComponentModel;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class GameFinishViewModel : ViewModelBase
{
    private int _timer;

    [ObservableProperty]
    private string winner;

    [ObservableProperty]
    private double progressTimer;

    public GameFinishViewModel(INavigationService navigationService, IToastService toastService)
        : base(navigationService, toastService)
    {
        Dispatcher.GetForCurrentThread().StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            _timer += 100;
            ProgressTimer = 100 * (double)_timer / 8000;
            if (_timer != 8000)
                return true;
            navigationService.NavigateToAsync(nameof(LoginViewModel));
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
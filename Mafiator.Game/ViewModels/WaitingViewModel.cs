using CommunityToolkit.Mvvm.ComponentModel;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class WaitingViewModel : ViewModelBase
{
    [ObservableProperty]
    private string message;

    public WaitingViewModel(INavigationService navigationService, IToastService toastService)
        : base(navigationService, toastService)
    {

    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is string data)
            Message = data;
        return base.InitializeAsync(navigationData);
    }
}
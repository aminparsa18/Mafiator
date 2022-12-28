using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels;

public class WaitingViewModel : ViewModelBase
{
    private string message;
    public string Message
    {
        get => message;
        set => SetProperty(ref message, value);
    }

    public WaitingViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService)
        : base(navigationService, localizer, toastService)
    {

    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is string data)
            Message = data;
        return base.InitializeAsync(navigationData);
    }
}
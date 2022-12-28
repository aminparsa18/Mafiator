using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels;

public class BarcodeScannerViewModel : ViewModelBase
{
    public BarcodeScannerViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService) 
        : base(navigationService, localizer, toastService)
    {
    }
}
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public class BarcodeScannerViewModel : ViewModelBase
{
    public BarcodeScannerViewModel(INavigationService navigationService, IToastService toastService) 
        : base(navigationService, toastService)
    {
    }
}
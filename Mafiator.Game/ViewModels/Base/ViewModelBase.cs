using CommunityToolkit.Mvvm.ComponentModel;
using Mafiator.Game.Services;

namespace Mafiator.Game.ViewModels.Base;

public partial class ViewModelBase : ObservableObject
{
    protected readonly INavigationService _navigationService;
    protected readonly IToastService _toastService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _pageTitle;

    protected ViewModelBase(INavigationService navigationService, IToastService toastService)
    {
        _navigationService = navigationService;
        _toastService = toastService;
    }

    public virtual Task InitializeAsync(object navigationData) => Task.FromResult(false);
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class InquiryStatusViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _inquiry;

    public IAsyncRelayCommand PopCommand { get; set; }

    public InquiryStatusViewModel(INavigationService navigationService, IToastService toastService)
        : base(navigationService, toastService)
    {
        PopCommand = new AsyncRelayCommand(Pop);
    }

    private async Task Pop()
    {
        await _navigationService.RemovePopupAsync();
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is bool data)
            Inquiry = data;
        return base.InitializeAsync(navigationData);
    }
}
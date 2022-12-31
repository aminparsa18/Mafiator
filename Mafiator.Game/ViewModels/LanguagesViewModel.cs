using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using System.Globalization;

namespace Mafiator.Game.ViewModels;

public partial class LanguagesViewModel : ViewModelBase
{
    private readonly IAsyncPublisher<ChangeLanguageEvent> _publisher;

    [ObservableProperty]
    private CountryResult _country;

    public ObservableRangeCollection<CountryResult> Countries { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }

    public LanguagesViewModel(INavigationService navigationService, IToastService toastService,
        IAsyncPublisher<ChangeLanguageEvent> publisher) : base(navigationService, toastService)
    {
        _publisher = publisher;
        Countries = new ObservableRangeCollection<CountryResult>()
        {
            new() { Sign = "US", Name = "English"},
            new() { Sign = "RU", Name = "Russian"}
        };
        PopCommand = new AsyncRelayCommand(Pop);
    }

    [RelayCommand]
    private async Task CountrySelected()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Setting Language");
        Barrel.Current.Add("Culture", Country.Sign, TimeSpan.MaxValue);
        if (Country.Sign == "RU")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU", false);
        }
        else
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
        }
        await _publisher.PublishAsync(new ChangeLanguageEvent());
        await _navigationService.RemovePopupAsync();
        await _navigationService.RemovePopupAsync();
    }

    private async Task Pop() => await _navigationService.RemovePopupAsync();
}
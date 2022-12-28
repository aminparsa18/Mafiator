using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Game.Models;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Mafiator.Game.ViewModels;

public class LanguagesViewModel : ViewModelBase
{
    public ObservableRangeCollection<Country> Countries { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }
    public IAsyncRelayCommand CountrySelectedCommand { get; set; }

    private Country country;
    public Country Country
    {
        get => country;
        set => SetProperty(ref country, value);
    }

    private readonly IAsyncPublisher<ChangeLanguageEvent> _publisher;

    public LanguagesViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
        IAsyncPublisher<ChangeLanguageEvent> publisher) : base(navigationService, localizer, toastService)
    {
        _publisher = publisher;
        Countries = new ObservableRangeCollection<Country>()
        {
            new() {Code = "US", Name = "English"},
            new() {Code = "RU", Name = "Russian"}
        };
        PopCommand = new AsyncRelayCommand(Pop);
        CountrySelectedCommand = new AsyncRelayCommand(CountrySelected);
    }

    private async Task CountrySelected()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Setting Language");
        Barrel.Current.Add("Culture", Country.Code, TimeSpan.MaxValue);
        if (Country.Code == "RU")
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
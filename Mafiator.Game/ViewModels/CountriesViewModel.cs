using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client;
using Mafiator.Game.Models;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace Mafiator.Game.ViewModels;

public class CountriesViewModel : ViewModelBase
{

    public ObservableRangeCollection<Country> Countries { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }
    public IAsyncRelayCommand LoadDataCommand { get; set; }
    public IAsyncRelayCommand CountrySelectedCommand { get; set; }

    private Country _country;
    public Country Country
    {
        get => _country;
        set => SetProperty(ref _country, value);
    }

    private readonly IPublisher<Country> _publisher;

    private List<Country> _countries;
    public CountriesViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer,
        IToastService toastService, IPublisher<Country> publisher) : base(navigationService, localizer, toastService)
    {
        _publisher = publisher;
        Countries = new ObservableRangeCollection<Country>();
        LoadDataCommand = new AsyncRelayCommand(LoadData);
        LoadDataCommand.ExecuteAsync(null);
        PopCommand = new AsyncRelayCommand(Pop);
        CountrySelectedCommand = new AsyncRelayCommand(CountrySelected);
    }

    private async Task CountrySelected()
    {
        _publisher.Publish(Country);
        await _navigationService.RemovePopupAsync();
    }

    private async Task Pop() => await _navigationService.RemovePopupAsync();

    private async Task LoadData()
    {
        var json = await BaseHttpClient.Instance.GetStringAsync("https://filebin.net/868ee5vt07j21ndf/countries.json");
        //var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MafiatorApp.countries.json");
        _countries = JsonSerializer.Deserialize<List<Country>>(json);
        Countries.AddRange(_countries);
    }

    public void Filter(string criteria)
    {
        Countries.Clear();
        Countries.AddRange(_countries.Where(c => c.Name.ToLower().Contains(criteria.ToLower())));
    }
}
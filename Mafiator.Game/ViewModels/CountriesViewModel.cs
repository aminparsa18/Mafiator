using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Countries;
using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;

namespace Mafiator.Game.ViewModels;

public partial class CountriesViewModel : ViewModelBase
{
    private List<CountryResult> _countries;

    private readonly ICountriesApiService _countriesApiService;
    private readonly IPublisher<CountryResult> _publisher;

    [ObservableProperty]
    private CountryResult _country;

    public ObservableRangeCollection<CountryResult> Countries { get; set; }

    public CountriesViewModel(INavigationService navigationService, IToastService toastService, ICountriesApiService countriesApiService,
        IPublisher<CountryResult> publisher) : base(navigationService, toastService)
    {
        _countriesApiService = countriesApiService;
        _publisher = publisher;
        _countries = new List<CountryResult>();
        Countries = new ObservableRangeCollection<CountryResult>();
        LoadDataCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task CountrySelected()
    {
        _publisher.Publish(Country);
        await _navigationService.RemovePopupAsync();
    }

    [RelayCommand]
    private async Task Pop() => await _navigationService.RemovePopupAsync();

    [RelayCommand]
    private async Task LoadData()
    {
        var res = await _countriesApiService.GetAllCountries();
        if (res.IsSuccess)
        {
            _countries.AddRange(res.Data);
            Countries.AddRange(_countries);
        }
    }

    public void Filter(string criteria)
    {
        Countries.Clear();
        Countries.AddRange(_countries.Where(c => c.Name.ToLower().Contains(criteria.ToLower())));
    }
}
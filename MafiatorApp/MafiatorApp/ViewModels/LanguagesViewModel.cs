using Mafiator.Common.Client.Cache;
using MafiatorApp.Models;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.ViewModels
{
    public class LanguagesViewModel : ViewModelBase
    {
        private readonly IAsyncPublisher<ChangeLanguageEvent> _publisher;
        public ObservableRangeCollection<Country> Countries { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public IAsyncCommand CountrySelectedCommand { get; set; }
        private Country country;
        public Country Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }

        public LanguagesViewModel(IAsyncPublisher<ChangeLanguageEvent> publisher)
        {
            this._publisher = publisher;
            Countries = new ObservableRangeCollection<Country>()
            {
                new() {Code = "US", Name = "English"},
                new() {Code = "RU", Name = "Russian"}
            };
            PopCommand = new AsyncCommand(Pop);
            CountrySelectedCommand = new AsyncCommand(CountrySelected);
        }

        private async Task CountrySelected()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Setting Language");
            Barrel.Current.Add("Culture", Country.Code, TimeSpan.MaxValue);
            if (Country.Code == "RU")
            {
                LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("ru-RU", false);
            }
            else
            {
                LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en-US", false);
            }
            await _publisher.PublishAsync(new ChangeLanguageEvent());
            await NavigationService.RemovePopupAsync();
            await NavigationService.RemovePopupAsync();
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }
    }
}
using System;
using System.Globalization;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Models;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Helpers;

namespace MafiatorApp.ViewModels
{
    public class LanguagesViewModel : ViewModelBase
    {
        private readonly IPublisher<ChangeLanguageEvent> _publisher;
        public ObservableRangeCollection<Country> Countries { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public IAsyncCommand CountrySelectedCommand { get; set; }
        private Country country;
        public Country Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }

        public LanguagesViewModel(IPublisher<ChangeLanguageEvent> publisher)
        {
            this._publisher = publisher;
            Countries = new Xamarin.CommunityToolkit.ObjectModel.ObservableRangeCollection<Country>()
            {
                new Country() {Code = "US", Name = "English"},
                new Country() {Code = "RU", Name = "Russian"}
            };
            PopCommand = new AsyncCommand(Pop);
            CountrySelectedCommand = new AsyncCommand(CountrySelected);
        }

        private async Task CountrySelected()
        {
            Barrel.Current.Add("Culture", Country.Code, TimeSpan.MaxValue);
            if (Country.Code == "RU")
            {
                LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("ru-RU", false);
            }
            else
            {
                LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en-US", false);
            }
            _publisher.Publish(new ChangeLanguageEvent());
            await NavigationService.RemovePopupAsync();
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

    }
}
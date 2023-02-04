using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using MafiatorApp.Models;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.ViewModels
{
    public class CountriesViewModel:ViewModelBase
    {
        private readonly IPublisher<Country> publisher;
        public ObservableRangeCollection<Country> Countries { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public IAsyncCommand LoadDataCommand { get; set; }
        public IAsyncCommand CountrySelectedCommand { get; set; }
        private Country country;
        public Country Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }

        private List<Country> countries;
        public CountriesViewModel(IPublisher<Country> publisher)
        {
            this.publisher = publisher;
            Countries=new ObservableRangeCollection<Country>();
            LoadDataCommand=new AsyncCommand(LoadData);
            LoadDataCommand.ExecuteAsync();
            PopCommand=new AsyncCommand(Pop);
            CountrySelectedCommand=new AsyncCommand(CountrySelected);
        }

        private async Task CountrySelected()
        {
            publisher.Publish(Country);
            await NavigationService.RemovePopupAsync();
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

        private async Task LoadData()
        {
            var sw = new Stopwatch();
            var json = await BaseHttpClient.Instance.GetStringAsync("https://filebin.net/868ee5vt07j21ndf/countries.json");
            sw.Start();
            //var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MafiatorApp.countries.json");
            countries = JsonSerializer.Deserialize<List<Country>>(json);
            var tt = sw.ElapsedMilliseconds;
            Countries.AddRange(countries);
            AppNotificationManager
        }

        public void Filter(string criteria)
        {
           Countries.Clear();
           Countries.AddRange(countries.Where(c=>c.Name.ToLower().Contains(criteria.ToLower())));
        }
    }
}

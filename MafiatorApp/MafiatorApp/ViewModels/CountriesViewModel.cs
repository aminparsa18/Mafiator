using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using MafiatorApp.Models;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using MessagePipe;

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
            set
            {
                country = value;
                RaisePropertyChanged(()=>Country);
            }
        }

        private List<Country> countries;
        public CountriesViewModel(IPublisher<Country> publisher)
        {
            this.publisher = publisher;
            Countries=new ObservableRangeCollection<Country>();
            LoadDataCommand=new AsyncCommand(LoadData);
            PopCommand=new AsyncCommand(Pop);
            LoadDataCommand.ExecuteAsync();
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
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MafiatorApp.countries.json");
            countries =await JsonSerializer.DeserializeAsync<List<Country>>(stream);
            Countries.AddRange(countries);
        }

        public void Filter(string criteria)
        {
           Countries.Clear();
           Countries.AddRange(countries.Where(c=>c.Name.ToLower().Contains(criteria.ToLower())));
        }
    }
}

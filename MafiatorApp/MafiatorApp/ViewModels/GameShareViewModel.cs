using System;
using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace MafiatorApp.ViewModels
{
    public class GameShareViewModel:ViewModelBase
    {
        private string date;
        public string Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
        private Ulid gameId;
        public Ulid GameId
        {
            get => gameId;
            set => SetProperty(ref gameId, value);
        }

        public IAsyncCommand ShareCommand { get; set; }

        public GameShareViewModel()
        {
            ShareCommand=new AsyncCommand(ShareGame);
        }

        private async Task ShareGame()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "کد بازی:"+GameId,
                Title = "اشتراک گذاری بازی"
            });
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Tuple<DateTime, Ulid> data)
            {
                Date = data.Item1.ToString();
                GameId = data.Item2;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}

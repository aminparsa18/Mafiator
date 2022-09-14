using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Enums;
using MafiatorApp.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.ViewModels
{
    public class RoomGamesViewModel:ViewModelBase
    {
        private bool noGame=true;

        public bool NoGame
        {
            get => noGame;
            set => SetProperty(ref noGame, value);
        }

        private Guid _roomId;
        public IAsyncCommand AddGameCommand { get; set; }

        private readonly IGamesApiService _gamesApiService;

        public RoomGamesViewModel(IGamesApiService gamesApiService)
        {
            _gamesApiService = gamesApiService;
            AddGameCommand=new AsyncCommand(AddGame);
        }

        private async Task AddGame()
        {
            await NavigationService.NavigateToAsync<NewGameViewModel>(_roomId);
        }

        private async Task LoadGames(string roomId)
        {
            var response=await _gamesApiService.GetGamesByRoom(roomId);
            if (response.IsSuccess)
            {
                if (response.Data.Any(s => s.Status == GameStatus.NotStarted))
                {
                    NoGame = false;
                }
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is Guid roomId)
            {
                _roomId = roomId;
                await LoadGames(roomId.ToString());
            }
        }
    }
}
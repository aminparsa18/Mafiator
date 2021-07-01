using System;
using System.Linq;
using System.Threading.Tasks;
using MafiatorApp.Enums;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;

namespace MafiatorApp.ViewModels
{
    public class RoomGamesViewModel:ViewModelBase
    {
        private bool noGame=true;

        public bool NoGame
        {
            get => noGame;
            set
            {
                noGame = value;
                RaisePropertyChanged(()=> NoGame);
            }
        }

        private Ulid _roomId;
        public IAsyncCommand AddGameCommand { get; set; }
        public RoomGamesViewModel()
        {
            AddGameCommand=new AsyncCommand(AddGame);
        }

        private async Task AddGame()
        {
            await NavigationService.NavigateToAsync<NewGameViewModel>(_roomId);
        }

        private async Task LoadGames(string roomId)
        {
            var response=await WebApiService.GetGamesByRoom(roomId);
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
            if (navigationData is Ulid roomId)
            {
                _roomId = roomId;
                await LoadGames(roomId.ToString());
            }
        }
    }
}

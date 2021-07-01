using System;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
   public class RandomViewModel:ViewModelBase
    {
        public ObservableRangeCollection<GameDto> Games { get; set; }
        public IAsyncCommand LoadGamesCommand { get; set; }
        public IAsyncCommand OpenRoomCommand { get; set; }
        public RandomViewModel()
        {
            Games=new ObservableRangeCollection<GameDto>();
            LoadGamesCommand=new AsyncCommand(LoadGames);
            LoadGamesCommand.ExecuteAsync();
            //OpenRoomCommand=new AsyncCommand(OpenRoom);
        }

        public async Task OpenRoom(Ulid roomId)
        {
            await NavigationService.NavigateToAsync<RoomDetailViewModel>(roomId);
        }

        private async Task LoadGames()
        {
            var response = await WebApiService.GetAvailableGames();
            if (response.IsSuccess)
            {
                Games.Clear();
                Games.AddRange(response.Data);
            }
            else
            {
                DependencyService.Get<IAlert>().ShortAlert(response.Errors.ToString(),MessageType.Error);
            }
        }
    }
}

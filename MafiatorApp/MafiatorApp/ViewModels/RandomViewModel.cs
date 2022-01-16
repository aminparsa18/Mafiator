using MafiatorApp.Dtos.Game;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
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

        public async Task OpenRoom(Guid roomId)
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

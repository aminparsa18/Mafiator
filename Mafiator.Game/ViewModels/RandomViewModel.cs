using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
{
    public class RandomViewModel : ViewModelBase
    {
        public ObservableRangeCollection<AvailableGameResult> Games { get; set; }
        public IAsyncRelayCommand LoadGamesCommand { get; set; }
        public IAsyncRelayCommand OpenRoomCommand { get; set; }

        private readonly IGamesApiService _gamesApiService;

        public RandomViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
            IGamesApiService gamesApiService) : base(navigationService, localizer, toastService)
        {
            _gamesApiService = gamesApiService;
            Games = new ObservableRangeCollection<AvailableGameResult>();
            LoadGamesCommand = new AsyncRelayCommand(LoadGames);
            LoadGamesCommand.ExecuteAsync(null);
            //OpenRoomCommand=new AsyncCommand(OpenRoom);
        }

        public async Task OpenRoom(Guid roomId)
        {
            await _navigationService.NavigateToAsync<RoomDetailViewModel>(roomId);
        }

        private async Task LoadGames()
        {
            var response = await _gamesApiService.GetAvailableGames();
            if (response.IsSuccess)
            {
                Games.Clear();
                Games.AddRange(response.Data);
            }
            else
            {
                _toastService.ShortAlert(response.Errors.ToString(), MessageType.Error);
            }
        }
    }
}
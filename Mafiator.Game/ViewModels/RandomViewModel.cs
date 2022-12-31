using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public class RandomViewModel : ViewModelBase
{
    private readonly IGamesApiService _gamesApiService;

    public ObservableRangeCollection<AvailableGameResult> Games { get; set; }
    public IAsyncRelayCommand LoadGamesCommand { get; set; }
    public IAsyncRelayCommand OpenRoomCommand { get; set; }

    public RandomViewModel(INavigationService navigationService, IToastService toastService, 
        IGamesApiService gamesApiService) : base(navigationService, toastService)
    {
        _gamesApiService = gamesApiService;
        Games = new ObservableRangeCollection<AvailableGameResult>();
        LoadGamesCommand = new AsyncRelayCommand(LoadGames);
        LoadGamesCommand.ExecuteAsync(null);
        //OpenRoomCommand=new AsyncCommand(OpenRoom);
    }

    public async Task OpenRoom(Guid roomId)
    {
        await _navigationService.NavigateToAsync($"{nameof(RoomDetailViewModel)}?roomId={roomId}");
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
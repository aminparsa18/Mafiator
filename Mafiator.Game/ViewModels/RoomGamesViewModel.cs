using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class RoomGamesViewModel : ViewModelBase
{
    private Guid _roomId;

    private readonly IGamesApiService _gamesApiService;

    [ObservableProperty]
    private bool noGame = true;

    public IAsyncRelayCommand AddGameCommand { get; set; }

    public RoomGamesViewModel(INavigationService navigationService, IToastService toastService, IGamesApiService gamesApiService) 
        : base(navigationService, toastService)
    {
        _gamesApiService = gamesApiService;
        AddGameCommand = new AsyncRelayCommand(AddGame);
    }

    private async Task AddGame() => await _navigationService.NavigateToAsync($"{nameof(NewGameViewModel)}?roomId={_roomId}");

    private async Task LoadGames(string roomId)
    {
        var response = await _gamesApiService.GetGamesByRoom(roomId);
        if (response.IsSuccess)
        {
            if (response.Data.Any(s => s.Status == GameStatus.NotStarted))
                NoGame = false;
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
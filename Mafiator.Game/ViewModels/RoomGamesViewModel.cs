using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels;

public class RoomGamesViewModel : ViewModelBase
{
    private bool noGame = true;
    public bool NoGame
    {
        get => noGame;
        set => SetProperty(ref noGame, value);
    }

    private Guid _roomId;
    public IAsyncRelayCommand AddGameCommand { get; set; }

    private readonly IGamesApiService _gamesApiService;

    public RoomGamesViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
        IGamesApiService gamesApiService) : base(navigationService, localizer, toastService)
    {
        _gamesApiService = gamesApiService;
        AddGameCommand = new AsyncRelayCommand(AddGame);
    }

    private async Task AddGame() => await _navigationService.NavigateToAsync<NewGameViewModel>(_roomId);

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
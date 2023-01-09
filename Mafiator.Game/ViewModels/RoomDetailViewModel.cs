using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Client.Services.RoomMembers;
using Mafiator.Common.Client.Services.Rooms;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;

namespace Mafiator.Game.ViewModels;

[QueryProperty(nameof(RoomId), "roomId")]
[QueryProperty(nameof(Room), "Room")]
public partial class RoomDetailViewModel : ViewModelBase
{
    private AppointedGameResult _appointedGame;

    private readonly IGamesApiService _gamesApiService;
    private readonly IRoomsApiService _roomsApiService;
    private readonly IRoomMembersApiService _roomMembersApiService;
    private readonly ISubscriber<UpdateRoomEvent> _subscriber;

    [ObservableProperty]
    private string _roomId;

    [ObservableProperty]
    private RoomDetailsResult _room;

    [ObservableProperty]
    private GameRoleDto _role;

    [ObservableProperty]
    private bool _newGameAvailable;

    [ObservableProperty]
    private bool _isJoined = true;

    [ObservableProperty]
    private bool _canLeave;

    [ObservableProperty]
    private string _remainingTime;

    [ObservableProperty]
    private LayoutState _currentState = LayoutState.Loading;

    public ObservableRangeCollection<RoomMemberResult> Members { get; set; }
    public ObservableRangeCollection<GameRoleDto> Roles { get; set; }

    public RoomDetailViewModel(INavigationService navigationService, IToastService toastService, 
        IGamesApiService gamesApiService, IRoomsApiService roomsApiService, IRoomMembersApiService roomMembersApiService, 
        ISubscriber<UpdateRoomEvent> subscriber) : base(navigationService, toastService)
    {
        _gamesApiService = gamesApiService;
        _roomsApiService = roomsApiService;
        _roomMembersApiService = roomMembersApiService;
        _subscriber = subscriber;
        Members = new ObservableRangeCollection<RoomMemberResult>();
        Roles = new ObservableRangeCollection<GameRoleDto>();
        _subscriber.Subscribe(async s => await LoadDataCommand.ExecuteAsync(null));
    }

    [RelayCommand]
    private async Task Chat() => await _navigationService.NavigateToAsync($"{nameof(ChatViewModel)}?roomId={_room.Id}");

    [RelayCommand]
    private async Task RoleSelected() => await _navigationService.NavigateToPopupAsync<PlayerRoleViewModel>(Tuple.Create(Role.Role, false));

    [RelayCommand]
    private async Task Join()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Joining Room...");
        var request = await _roomsApiService.JoinRoom(Room.Code);
        if (request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadAsMemoryPackAsync<ApiResult<string>>();
            if (result.IsSuccess)
                await LoadDataCommand.ExecuteAsync(null);
            else
            {
                _toastService.ShortAlert(result.Errors.FirstOrDefault(), MessageType.Error);
            }
        }
        else
        {
            var result = await request.Content.ReadAsStringAsync();
            _toastService.ShortAlert(result, MessageType.Error);
        }

        await _navigationService.RemovePopupAsync();
    }

    [RelayCommand]
    private async Task Leave()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Leaving...");
        var response = await _roomsApiService.LeaveRoom(new LeaveRoomRequest
        {
            RoomId = Room.Id
        });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsMemoryPackAsync<ApiResult>();
            if (result.IsSuccess)
            {
                _toastService.ShortAlert("You left the room", MessageType.Success);
                await _navigationService.RemoveLastFromBackStackAsync();
            }
            else
                _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
        }
        else
        {
            var result = await response.Content.ReadAsStringAsync();
            _toastService.ShortAlert(result, MessageType.Error);
        }

        await _navigationService.RemovePopupAsync();
    }

    [RelayCommand]
    private async Task GoToGame() => await _navigationService.NavigateToAsync(nameof(WaitingGameViewModel), new Dictionary<string, object>
    {
        ["AppointedGame"] = _appointedGame
    });

    [RelayCommand]
    private async Task NewGame() => await _navigationService.NavigateToPopupAsync<NewGameViewModel>(Room.Id);

    [RelayCommand]
    private async Task Copy()
    {
        await Clipboard.SetTextAsync(Room.Code);
        _toastService.ShortAlert("Code copied to clipboard", MessageType.Success);
    }

    [RelayCommand]
    private async Task Share()
    {
        await Microsoft.Maui.ApplicationModel.DataTransfer.Share.RequestAsync(new ShareTextRequest()
        {
            Subject = "Join Room",
            Text = "Join Mafiator Room",
            Title = "join",
            Uri = "http://invite.mafiator.com/joinRoom/" + Room.Id
        });
    }

    [RelayCommand]
    private async Task AddMember() => await _navigationService.NavigateToPopupAsync<NewMemberViewModel>(Room.Id);

    [RelayCommand]
    private async Task LoadData()
    {
        var members = await _roomMembersApiService.GetMembersByRoom(Room.Id);
        if (members.IsSuccess)
        {
            Members.Clear();
            Members.AddRange(members.Data);
        }

        var response = await _gamesApiService.GetAppointedGame(Room.Id.ToString());
        if (response.IsSuccess)
        {
            _appointedGame = response.Data;
            CurrentState = _appointedGame == null ? LayoutState.Empty : LayoutState.Success;
            if (_appointedGame == null && Room.IsAdmin)
                NewGameAvailable = true;
            else
                NewGameAvailable = false;
            if (_appointedGame != null)
            {
                RemainingTime = _appointedGame.Status == GameStatus.NotStarted
                    ? LocalizationResourceManager.Instance["NewGameIsWaiting"]
                    : LocalizationResourceManager.Instance["GameIsBeingPlayed"];
                Roles.AddRange(_appointedGame.Roles.Select(s => new GameRoleDto()
                {
                    Role = s
                }));
            }
        }
        else
        {
            CurrentState = LayoutState.Error;
        }

        if (Room.IsAdmin)
        {
            IsJoined = true;
            CanLeave = true;
        }
        else
        {
            var joinResult = await _roomsApiService.IsRoomJoined(Room.Id.ToString());
            if (joinResult.IsSuccess)
            {
                IsJoined = !string.IsNullOrEmpty(joinResult.Data);
                CanLeave = IsJoined;
            }
        }
    }

    public async Task InitializeAsync()
    {
        if (!string.IsNullOrEmpty(RoomId))
        {
            ApiResult<RoomDetailsResult> response = await _roomsApiService.GetRoom(RoomId.ToString());
            if (response.IsSuccess)
            {
                response.Data.Id = Guid.Parse(RoomId);
                Room = response.Data;
            }
            else
                _toastService.ShortAlert(response.Errors.ToString(), MessageType.Error);
        }

        await LoadData();
    }
}
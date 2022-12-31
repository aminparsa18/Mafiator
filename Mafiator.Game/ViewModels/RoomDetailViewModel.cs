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
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;

namespace Mafiator.Game.ViewModels;

public partial class RoomDetailViewModel : ViewModelBase
{
    private ApiResult<AppointedGameResult> waiting;

    private readonly IGamesApiService _gamesApiService;
    private readonly IRoomsApiService _roomsApiService;
    private readonly IRoomMembersApiService _roomMembersApiService;
    private readonly ISubscriber<UpdateRoomEvent> _subscriber;

    [ObservableProperty]
    private RoomDetailsResult room;

    [ObservableProperty]
    private GameRoleDto role;

    [ObservableProperty]
    private bool newGameAvailable;

    [ObservableProperty]
    private bool isJoined = true;

    [ObservableProperty]
    private bool canLeave;

    [ObservableProperty]
    private string remainingTime;

    [ObservableProperty]
    private LayoutState currentState;

    public IAsyncRelayCommand LoadDataCommand { get; set; }
    public IAsyncRelayCommand AddMemberCommand { get; set; }
    public IAsyncRelayCommand CopyCommand { get; set; }
    public IAsyncRelayCommand QrCommand { get; set; }
    public IAsyncRelayCommand NewGameCommand { get; set; }
    public IAsyncRelayCommand ChatCommand { get; set; }
    public IAsyncRelayCommand GoToGameCommand { get; set; }
    public IAsyncRelayCommand RoleChangeCommand { get; set; }
    public IAsyncRelayCommand JoinCommand { get; set; }
    public IAsyncRelayCommand LeaveCommand { get; set; }
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
        LoadDataCommand = new AsyncRelayCommand(LoadData);
        AddMemberCommand = new AsyncRelayCommand(AddMember);
        CopyCommand = new AsyncRelayCommand(Copy);
        QrCommand = new AsyncRelayCommand(GenerateQr);
        NewGameCommand = new AsyncRelayCommand(StartNewGame);
        GoToGameCommand = new AsyncRelayCommand(GoToGame);
        LeaveCommand = new AsyncRelayCommand(Leave);
        JoinCommand = new AsyncRelayCommand(Join);
        RoleChangeCommand = new AsyncRelayCommand(RoleChanged);
        ChatCommand = new AsyncRelayCommand(Chat);
        _subscriber.Subscribe(async s => await LoadDataCommand.ExecuteAsync(null));
    }

    private async Task Chat() => await _navigationService.NavigateToAsync($"{nameof(ChatViewModel)}?roomId={room.Id}");

    private async Task RoleChanged() => await _navigationService.NavigateToPopupAsync<PlayerRoleViewModel>(Tuple.Create(Role.Role, false));

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

    private async Task GoToGame() => await _navigationService.NavigateToAsync(nameof(WaitingGameViewModel), new Dictionary<string, object>
    {
        ["AppointedGame"] = waiting.Data
    });

    private async Task StartNewGame() => await _navigationService.NavigateToPopupAsync<NewGameViewModel>(Room.Id);

    private async Task Copy()
    {
        await Clipboard.SetTextAsync(Room.Code);
        _toastService.ShortAlert("Code copied to clipboard", MessageType.Success);
    }

    private async Task GenerateQr()
    {
        await Share.RequestAsync(new ShareTextRequest()
        {
            Subject = "Join Room",
            Text = "Join Mafiator Room",
            Title = "join",
            Uri = "http://invite.mafiator.com/joinRoom/" + Room.Id
        });
    }

    private async Task AddMember() => await _navigationService.NavigateToPopupAsync<NewMemberViewModel>(Room.Id);

    private async Task LoadData()
    {
        CurrentState = LayoutState.Loading;
        var members = await _roomMembersApiService.GetMembersByRoom(Room.Id);
        if (members.IsSuccess)
        {
            Members.Clear();
            Members.AddRange(members.Data);
        }

        waiting = await _gamesApiService.GetAppointedGame(Room.Id.ToString());
        if (waiting.IsSuccess)
        {
            CurrentState = waiting.Data == null ? LayoutState.Empty : LayoutState.Success;
            if (waiting.Data == null && Room.IsAdmin)
                NewGameAvailable = true;
            else
                newGameAvailable = false;
            if (waiting.Data != null)
            {
                RemainingTime = waiting.Data.Status == GameStatus.NotStarted
                    ? "New game is waiting for members to start"
                    : "Game is being played";
                Roles.AddRange(waiting.Data.Roles.Select(s => new GameRoleDto()
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

    public override async Task InitializeAsync(object navigationData)
    {
        if (navigationData is Guid roomId)
        {
            ApiResult<RoomDetailsResult> response = await _roomsApiService.GetRoom(roomId.ToString());
            if (response.IsSuccess)
            {
                response.Data.Id = roomId;
                Room = response.Data;
            }
            else
                _toastService.ShortAlert(response.Errors.ToString(), MessageType.Error);
        }
        else if (navigationData is RoomDetailsResult navigatedRoom)
            Room = navigatedRoom;

        await LoadData();
    }
}
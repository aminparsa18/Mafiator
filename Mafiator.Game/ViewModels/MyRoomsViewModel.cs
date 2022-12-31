using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Rooms;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class MyRoomsViewModel : ViewModelBase
{
    private readonly IRoomsApiService _roomsApiService;

    [ObservableProperty]
    private LayoutState _currentState;

    [ObservableProperty]
    private RoomDetailsResult _room;

    public ObservableRangeCollection<RoomDetailsResult> Rooms { get; set; }
    public IAsyncRelayCommand LoadRoomsCommand { get; set; }
    public IAsyncRelayCommand RoomSelectedCommand { get; set; }
    public IAsyncRelayCommand AddRoomCommand { get; set; }

    public MyRoomsViewModel(INavigationService navigationService, IToastService toastService, 
        IRoomsApiService roomsApiService) : base(navigationService, toastService)
    {
        _roomsApiService = roomsApiService;
        Rooms = new ObservableRangeCollection<RoomDetailsResult>();
        LoadRoomsCommand = new AsyncRelayCommand(LoadRooms);
        RoomSelectedCommand = new AsyncRelayCommand(RoomSelected);
        AddRoomCommand = new AsyncRelayCommand(AddRoom);
    }

    private async Task AddRoom() => await _navigationService.NavigateToPopupAsync<NewRoomViewModel>();

    private async Task RoomSelected()
    {
        if (Room == null)
            return;
        await _navigationService.NavigateToAsync(nameof(RoomDetailViewModel), new Dictionary<string, object> { 
            ["Room"] = Room
        });
        Room = null;
    }

    private async Task LoadRooms()
    {
        CurrentState = LayoutState.Loading;
        var rooms = await _roomsApiService.GetMyRooms();
        if (rooms.IsSuccess)
        {
            Rooms.Clear();
            Rooms.AddRange(rooms.Data);
            CurrentState = !Rooms.Any() ? LayoutState.Empty : LayoutState.Success;
        }
        else
        {
            CurrentState = LayoutState.Error;
            _toastService.ShortAlert(rooms.Errors.ToString(), MessageType.Error);
        }
    }

    public async Task Leave(string roomId)
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Leaving...");
        var response = await _roomsApiService.LeaveRoom(new LeaveRoomRequest
        {
            RoomId = Guid.Parse(roomId)
        });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsMemoryPackAsync<ApiResult>();
            if (result.IsSuccess)
                await LoadRoomsCommand.ExecuteAsync(null);
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
}
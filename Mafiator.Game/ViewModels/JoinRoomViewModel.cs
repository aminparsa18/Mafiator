using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Rooms;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels;

public class JoinRoomViewModel : ViewModelBase
{
    private ValidatableObject<string> _code;
    public ValidatableObject<string> Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }

    public IAsyncRelayCommand PopCommand { get; set; }
    public IAsyncRelayCommand JoinRoomCommand { get; set; }

    private readonly IRoomsApiService _roomsApiService;

    public JoinRoomViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService,
        IRoomsApiService roomsApiService) : base(navigationService, localizer, toastService)
    {
        _roomsApiService = roomsApiService;
        Code = new ValidatableObject<string>();
        PopCommand = new AsyncRelayCommand(Pop);
        JoinRoomCommand = new AsyncRelayCommand(JoinRoom);
    }

    private async Task JoinRoom()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Joining Room...");
        var request = await _roomsApiService.JoinRoom(Code.Value);
        if (request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadAsMessagePackAsync<ApiResult<string>>();
            if (result.IsSuccess)
            {
                await _navigationService.RemovePopupAsync();
                await _navigationService.RemovePopupAsync();
                await _navigationService.NavigateToAsync<RoomDetailViewModel>(Guid.Parse(result.Data));
            }
            else
            {
                await _navigationService.RemovePopupAsync();
                _toastService.ShortAlert(result.Errors.FirstOrDefault(), MessageType.Error);
            }
        }
        else
        {
            var result = await request.Content.ReadAsStringAsync();
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert(result, MessageType.Error);
        }
    }

    private async Task Pop() => await _navigationService.RemovePopupAsync();
}
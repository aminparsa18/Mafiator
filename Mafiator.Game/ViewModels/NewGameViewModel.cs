using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Game.Models;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Plugin.MauiMTAdmob;

namespace Mafiator.Game.ViewModels;

public partial class NewGameViewModel : ViewModelBase
{
    private Guid _roomId;

    private readonly IGamesApiService _gamesApiService;
    private readonly IPublisher<UpdateRoomEvent> _publisher;
    
    [ObservableProperty]
    private ValidatableObject<short> _capacity;

    [ObservableProperty]
    private bool _isCapacityValid;

    [ObservableProperty]
    private bool _isImmediate;

    [ObservableProperty]
    private bool _isPublic;

    [ObservableProperty]
    private DateTime? _date = DateTime.Now;

    public ObservableRangeCollection<NewGameRole> Roles { get; set; }

    public NewGameViewModel(INavigationService navigationService, IToastService toastService,
        IGamesApiService gamesApiService, IPublisher<UpdateRoomEvent> publisher) : base(navigationService, toastService)
    {
        _gamesApiService = gamesApiService;
        _publisher = publisher;
        Capacity = new ValidatableObject<short> { Value = 6 };
        Roles = new ObservableRangeCollection<NewGameRole>();
    }

    [RelayCommand]
    private void PublicHelp() => _toastService.ShortAlert("Everyone can observe your game live as guests", MessageType.Info);

    [RelayCommand]
    private async Task Pop()
    {
        SystemConstant.SelectedRoles = null;
        await _navigationService.RemovePopupAsync();
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is Guid roomId)
            _roomId = roomId;

        return base.InitializeAsync(navigationData);
    }

    [RelayCommand]
    private async Task SaveGame()
    {
        if (Capacity.Value < 6)
        {
            _toastService.ShortAlert("Game Players must be at least 6 person", MessageType.Error);
            return;
        }

        if (SystemConstant.SelectedRoles == null || !SystemConstant.SelectedRoles.Any())
        {
            await _navigationService.NavigateToPopupAsync<SetRolesViewModel>(Capacity.Value);
            return;
        }

        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Creating Game...");
        var response = await _gamesApiService.AddGame(new GameCreateRequest()
        {
            RoomId = _roomId,
            Roles = SystemConstant.SelectedRoles.Select(s => new GameRoleCreateRequest() { Role = s.Role, Count = s.Count })
                .ToList(),
            StartDate = IsImmediate ? DateTime.Now : Date.Value
        });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsMemoryPackAsync<ApiResult<GameCreateResult>>();
            if (result.IsSuccess)
            {
                SystemConstant.SelectedRoles = null;
                _publisher.Publish(new UpdateRoomEvent());
                CrossMauiMTAdmob.Current.LoadInterstitial("");
                await _navigationService.RemovePopupAsync();
            }
            else
            {
                _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
            }
        }
        else
        {
            var result = await response.Content.ReadAsStringAsync();
            _toastService.ShortAlert("result.Errors[0]", MessageType.Error);
        }

        await _navigationService.RemovePopupAsync();
    }

    [RelayCommand]
    private async Task SetRoles()
    {
        if (Capacity.Value < 6)
            _toastService.ShortAlert("Game Players must be at least 6 person", MessageType.Error);
        else
            await _navigationService.NavigateToPopupAsync<SetRolesViewModel>(Capacity.Value);
    }


    public void SetTime(in TimeSpan time)
    {
        if (!Date.HasValue)
            return;
        Date = Date.Value.Date + time;
    }
}
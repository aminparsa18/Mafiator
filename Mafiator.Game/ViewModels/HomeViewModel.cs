using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;

namespace Mafiator.Game.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private UserDetailsResult _user = Barrel.Current.Get<UserDetailsResult>("User");

    [ObservableProperty]
    private UserStatusResult _userStatus;

    private readonly IAsyncSubscriber<UpdateProfileEvent> _subscriber;
    private readonly IUsersApiService _usersApiService;
    private readonly IDisposable _disposable;

    public HomeViewModel(INavigationService navigationService, IToastService toastService,
        IAsyncSubscriber<UpdateProfileEvent> subscriber, IUsersApiService usersApiService) : base(navigationService, toastService)
    {
        _subscriber = subscriber;
        _usersApiService = usersApiService;
        var bag = DisposableBag.CreateBuilder();
        _subscriber.Subscribe(async (_, _) => await LoadData()).AddTo(bag);
        _disposable = bag.Build();
        LoadDataCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task EditProfile() => await _navigationService.NavigateToAsync($"{nameof(ProfilePictureViewModel)}?isEdit={true}");

    [RelayCommand]
    private async Task Store() => await _navigationService.NavigateToAsync(nameof(StoreViewModel));

    [RelayCommand]
    private static async Task Help() => await Launcher.OpenAsync(new Uri("https://mafiator.com/game-en.pdf"));

    [RelayCommand]
    private async Task Settings() => await _navigationService.NavigateToPopupAsync<SettingsViewModel>();

    [RelayCommand]
    private async Task SignOut()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Instance["SigningOut"]);
        Barrel.Current.Empty("Token");
        Barrel.Current.Empty("User");
        BaseHttpClient.Instance.DefaultRequestHeaders.Authorization = null;
        await _navigationService.RemovePopupAsync();
        await _navigationService.NavigateToAsync(nameof(LoginViewModel));
    }

    [RelayCommand]
    private async Task AddRoom() => await _navigationService.NavigateToPopupAsync<NewRoomViewModel>();

    [RelayCommand]
    private async Task RoomHistory() => await _navigationService.NavigateToAsync(nameof(MyRoomsViewModel));

    [RelayCommand]
    private async Task JoinRoom() => await _navigationService.NavigateToPopupAsync<JoinRoomViewModel>();

    [RelayCommand]
    private async Task Random() => await _navigationService.NavigateToAsync(nameof(RandomViewModel));

    [RelayCommand]
    private async Task LoadData()
    {
        if (!Barrel.Current.Exists("User") || Barrel.Current.IsExpired("User"))
        {
            var userResponse = await _usersApiService.GetUser();
            if (userResponse.IsSuccess)
            {
                User = userResponse.Data;
                Barrel.Current.Add("User", userResponse.Data, TimeSpan.FromDays(180));
            }
        }
        else
            User = Barrel.Current.Get<UserDetailsResult>("User");

        var response = await _usersApiService.GetUserStatus();
        if (response.IsSuccess)
            UserStatus = response.Data;
        else
            _toastService.ShortAlert(response.Errors.ToString(), MessageType.Error);
    }
}
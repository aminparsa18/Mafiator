using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private UserDetailsResult user = Barrel.Current.Get<UserDetailsResult>("User");

        public UserDetailsResult User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private RoomDetailsResult room;

        public RoomDetailsResult Room
        {
            get => room;
            set => SetProperty(ref room, value);
        }

        public UserStatusResult UserStatus { get; set; }
        public IAsyncRelayCommand LoadDataCommand { get; set; }
        public IAsyncRelayCommand AddRoomCommand { get; set; }
        public IAsyncRelayCommand RandomCommand { get; set; }
        public IAsyncRelayCommand JoinRoomCommand { get; set; }
        public IAsyncRelayCommand RoomHistoryCommand { get; set; }
        public IAsyncRelayCommand StoreCommand { get; set; }
        public IAsyncRelayCommand SettingsCommand { get; set; }
        public IAsyncRelayCommand HelpCommand { get; set; }
        public IAsyncRelayCommand EditProfileCommand { get; set; }
        public IAsyncRelayCommand SignOutCommand { get; set; }

        private readonly ISubscriber<UpdateProfileEvent> _subscriber;
        private readonly IUsersApiService _usersApiService;
        private readonly IDisposable _disposable;

        public HomeViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService,
            ISubscriber<UpdateProfileEvent> subscriber, IUsersApiService usersApiService) : base(navigationService, localizer, toastService)
        {
            _subscriber = subscriber;
            _usersApiService = usersApiService;
            var bag = DisposableBag.CreateBuilder();
            _subscriber.Subscribe(c => LoadData()).AddTo(bag);
            _disposable = bag.Build();
            LoadDataCommand = new AsyncRelayCommand(LoadData);
            AddRoomCommand = new AsyncRelayCommand(AddRoom);
            RandomCommand = new AsyncRelayCommand(Random);
            JoinRoomCommand = new AsyncRelayCommand(JoinRoom);
            RoomHistoryCommand = new AsyncRelayCommand(RoomHistory);
            StoreCommand = new AsyncRelayCommand(Store);
            SettingsCommand = new AsyncRelayCommand(Settings);
            HelpCommand = new AsyncRelayCommand(Help);
            EditProfileCommand = new AsyncRelayCommand(EditProfile);
            SignOutCommand = new AsyncRelayCommand(SignOut);
            LoadDataCommand.ExecuteAsync(null);
        }

        private async Task EditProfile()
        {
            await _navigationService.NavigateToAsync<ProfilePictureViewModel>(parameter: true);
        }

        private async Task Store()
        {
            await _navigationService.NavigateToAsync<StoreViewModel>();
        }

        private static async Task Help()
        {
            await Launcher.OpenAsync(new Uri("https://mafiator.com/game-en.pdf"));
        }

        private async Task Settings()
        {
            await _navigationService.NavigateToPopupAsync<SettingsViewModel>();
        }

        private async Task SignOut()
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Signing Out...");
            Barrel.Current.Empty("Token");
            Barrel.Current.Empty("User");
            BaseHttpClient.Instance.DefaultRequestHeaders.Authorization = null;
            await _navigationService.RemovePopupAsync();
            await _navigationService.NavigateToAsync<LoginViewModel>(true);
        }

        private async Task AddRoom()
        {
            await _navigationService.NavigateToPopupAsync<NewRoomViewModel>();
        }

        private async Task RoomHistory()
        {
            await _navigationService.NavigateToAsync<MyRoomsViewModel>();
        }

        private async Task JoinRoom()
        {
            await _navigationService.NavigateToPopupAsync<JoinRoomViewModel>();
        }


        private async Task Random()
        {
            await _navigationService.NavigateToAsync<RandomViewModel>();
        }

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
}
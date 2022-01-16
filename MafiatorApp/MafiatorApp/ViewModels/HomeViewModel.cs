using MafiatorApp.Cache;
using MafiatorApp.Dtos.Room;
using MafiatorApp.Dtos.User;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.Views;
using MessagePipe;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private UserDto user = Barrel.Current.Get<UserDto>("User");

        public UserDto User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private RoomDto room;

        public RoomDto Room
        {
            get => room;
            set => SetProperty(ref room, value);
        }

        public UserStatusDto UserStatus { get; set; }
        public IAsyncCommand AddRoomCommand { get; set; }
        public IAsyncCommand RandomCommand { get; set; }
        public IAsyncCommand JoinRoomCommand { get; set; }
        public IAsyncCommand RoomHistoryCommand { get; set; }
        public IAsyncCommand StoreCommand { get; set; }
        public IAsyncCommand SettingsCommand { get; set; }
        public IAsyncCommand HelpCommand { get; set; }
        public IAsyncCommand EditProfileCommand { get; set; }
        public IAsyncCommand SignOutCommand { get; set; }
        private readonly ISubscriber<UpdateProfileEvent> _subscriber;
        private readonly IDisposable _disposable;

        public HomeViewModel(ISubscriber<UpdateProfileEvent> subscriber)
        {
            _subscriber = subscriber;
            var bag = DisposableBag.CreateBuilder();
            _subscriber.Subscribe(c => LoadData()).AddTo(bag);
            _disposable = bag.Build();
            LoadData();
            AddRoomCommand = new AsyncCommand(AddRoom);
            RandomCommand = new AsyncCommand(Random);
            JoinRoomCommand = new AsyncCommand(JoinRoom);
            RoomHistoryCommand = new AsyncCommand(RoomHistory);
            StoreCommand = new AsyncCommand(Store);
            SettingsCommand = new AsyncCommand(Settings);
            HelpCommand = new AsyncCommand(Help);
            EditProfileCommand = new AsyncCommand(EditProfile);
            SignOutCommand = new AsyncCommand(SignOut);
        }

        private async Task EditProfile()
        {
            await NavigationService.NavigateToAsync<ProfilePictureViewModel>(true);
        }

        private async Task Store()
        {
            await NavigationService.NavigateToAsync<StoreViewModel>();
        }

        private static async Task Help()
        {
            await Launcher.OpenAsync(new Uri("https://mafiator.com/game-en.pdf"));
        }

        private async Task Settings()
        {
            await NavigationService.NavigateToPopupAsync<SettingsViewModel>();
        }

        private async Task SignOut()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Signing Out...");
            Barrel.Current.Empty("Token");
            Barrel.Current.Empty("User");
            await NavigationService.RemovePopupAsync();
            Application.Current.MainPage = new NavigationPage(new LoginView());
        }

        private async Task AddRoom()
        {
            await NavigationService.NavigateToPopupAsync<NewRoomViewModel>();
        }

        private async Task RoomHistory()
        {
            await NavigationService.NavigateToAsync<MyRoomsViewModel>();
        }

        private async Task JoinRoom()
        {
            await NavigationService.NavigateToPopupAsync<JoinRoomViewModel>();
        }


        private async Task Random()
        {
            await NavigationService.NavigateToAsync<RandomViewModel>();
        }

        private async void LoadData()
        {
            if (!Barrel.Current.Exists("User") || Barrel.Current.IsExpired("User"))
            {
                var userResponse = await WebApiService.GetUser();
                if (userResponse.IsSuccess)
                {
                    User = userResponse.Data;
                    Barrel.Current.Add("User", userResponse.Data, TimeSpan.FromDays(180));
                }
            }
            else
            {
                User = Barrel.Current.Get<UserDto>("User");
            }

            var response = await WebApiService.GetUserStatus();
            if (response.IsSuccess)
            {
                UserStatus = response.Data;
            }
            else
            {
                DependencyService.Get<IAlert>().ShortAlert(response.Errors.ToString(), MessageType.Error);
            }
        }
    }
}
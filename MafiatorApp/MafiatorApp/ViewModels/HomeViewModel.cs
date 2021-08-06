using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Grpc.Core;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Helpers;
using MafiatorApp.Models;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.Views;
using MessagePipe;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
//using MagicOnion.Client;
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

        private async Task Help()
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
            // var channel = GrpcChannel.ForAddress("https://mftor.ir");//
            //   var channel=new Channel("mftor.ir:443", ChannelCredentials.Insecure);
            //  var channel =new Channel("mftor.ir:443",new SslCredentials("-----BEGIN CERTIFICATE-----\r\nMIIFUzCCBDugAwIBAgISA+JR39lWm334FghCem5om1ECMA0GCSqGSIb3DQEBCwUA\r\nMEoxCzAJBgNVBAYTAlVTMRYwFAYDVQQKEw1MZXQncyBFbmNyeXB0MSMwIQYDVQQD\r\nExpMZXQncyBFbmNyeXB0IEF1dGhvcml0eSBYMzAeFw0yMDEyMDEyMTM1NThaFw0y\r\nMTAzMDEyMTM1NThaMBgxFjAUBgNVBAMTDWdycGMubWZ0b3IuaXIwggEiMA0GCSqG\r\nSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDCUA16H2N11g7VaE+s00QEfMVs5EhNrtSY\r\nW8A5I83cefjAEv0q2jzjpQvrgvXBgnr52OsZno0qg5FkfjG2re3CzTEVZ0Ewt1ON\r\n4P+PRXuKYo4vyvfuijAlA+BX7cV+4C9kh+H1X1VNAd0fkWyFxhCpQvN9AylXuj6O\r\n0r7MJFdhcUVJJHiohuPsbkwKkTlDjVddS7aYI7IY8ccCAbNkCK8HJEvHA6vhqr5o\r\n/7vkFxDWz2p9gYV380OnRRuf7EouwPVYtibrvCc002bV+WZp/F2X3Qj1myv5S5gm\r\nxKz5PUYVnIx8y1hLNm/lt7wHabSXMj53teR4WAso+oAXOOTdyWRbAgMBAAGjggJj\r\nMIICXzAOBgNVHQ8BAf8EBAMCBaAwHQYDVR0lBBYwFAYIKwYBBQUHAwEGCCsGAQUF\r\nBwMCMAwGA1UdEwEB/wQCMAAwHQYDVR0OBBYEFGVyOZdD7MGIUHT5qIaAnx9k1Wl6\r\nMB8GA1UdIwQYMBaAFKhKamMEfd265tE5t6ZFZe/zqOyhMG8GCCsGAQUFBwEBBGMw\r\nYTAuBggrBgEFBQcwAYYiaHR0cDovL29jc3AuaW50LXgzLmxldHNlbmNyeXB0Lm9y\r\nZzAvBggrBgEFBQcwAoYjaHR0cDovL2NlcnQuaW50LXgzLmxldHNlbmNyeXB0Lm9y\r\nZy8wGAYDVR0RBBEwD4INZ3JwYy5tZnRvci5pcjBMBgNVHSAERTBDMAgGBmeBDAEC\r\nATA3BgsrBgEEAYLfEwEBATAoMCYGCCsGAQUFBwIBFhpodHRwOi8vY3BzLmxldHNl\r\nbmNyeXB0Lm9yZzCCAQUGCisGAQQB1nkCBAIEgfYEgfMA8QB3AFzcQ5L+5qtFRLFe\r\nmtRW5hA3+9X6R9yhc5SyXub2xw7KAAABdiByujQAAAQDAEgwRgIhAKnkapx65W16\r\nutFvZbZfn0yTb2K4h3YOu3larW2tQlYIAiEA1mFLnBvYdZt1a4bMt5WwN3bnpFBs\r\njUFppyttlmyE2hkAdgB9PvL4j/+IVWgkwsDKnlKJeSvFDngJfy5ql2iZfiLw1wAA\r\nAXYgcrp4AAAEAwBHMEUCIQCom7v3jjwOXFzuZmURGPR6tQ361GxWzuDtXkQrlQJ0\r\nJAIgaKfo+Y5dmJ30mzROjRDHmTltuFiBkYOTSL81PAAgaGAwDQYJKoZIhvcNAQEL\r\nBQADggEBAAFBa2+HcjOUjfLQUwLza9MYdL10dPbWckbRPJ5XOTZMvzuc57JbSh+3\r\nYN20wi3MONJGxOQDbdjhHP8wRx6gm9mBLIIhxW4D6SO5qtrk2slaccMLWdNzgBIj\r\nIFgsbBa4jyD382rpz1GF5NKlKoEhrhwb25Uu/yMPKIlqO3hPpTtQvxcSkAhsXzZF\r\nVYRBXUWIuTzsS64fxHQsjAhF+lJn3omsog5c24zEzu0wc0MNvwxT8A8zCy/xHveS\r\n9sn2D64TT24UYeG/5HqQ/r4/YFErCOizXZquJEkm9iUa8K75HGXEIqfnODowRukX\r\nxKFLh8+/RlvaT17505egmoJleRjNnSw=\r\n-----END CERTIFICATE-----")); //.ForAddress("https://grpc.mftor.ir");

            // NOTE: If your project targets non-.NET Standard 2.1, use `Grpc.Core.Channel` class instead.
            // var channel = new Channel("localhost", 5001, new SslCredentials());

            // Create a proxy to call the server transparently.
            //  var client = MagicOnionClient.Create<ITestGrpc>(channel.CreateCallInvoker());
            // Call the server-side method using the proxy.
            // var result = await client.SumAsync(123, 456);
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
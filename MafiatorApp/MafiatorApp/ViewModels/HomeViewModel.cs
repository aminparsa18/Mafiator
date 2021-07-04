using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Grpc.Core;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Helpers;
using MafiatorApp.Models;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using MafiatorApp.Views;
using MarcTron.Plugin;
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
            set
            {
                user = value;
                RaisePropertyChanged(() => User);
            }
        }

        private RoomDto room;

        public RoomDto Room
        {
            get => room;
            set
            {
                room = value;
                RaisePropertyChanged(() => Room);
            }
        }

        public UserStatusDto UserStatus { get; set; }
        public List<Tip> Tips { get; set; }
        public IAsyncCommand AddRoomCommand { get; set; }
        public IAsyncCommand RandomCommand { get; set; }
        public IAsyncCommand JoinRoomCommand { get; set; }
        public IAsyncCommand RoomHistoryCommand { get; set; }
        public IAsyncCommand SettingsCommand { get; set; }
        public IAsyncCommand SignOutCommand { get; set; }

        public HomeViewModel()
        {
            LoadData();
            AddRoomCommand = new AsyncCommand(AddRoom);
            RandomCommand = new AsyncCommand(Random);
            JoinRoomCommand = new AsyncCommand(JoinRoom);
            RoomHistoryCommand = new AsyncCommand(RoomHistory);
            SettingsCommand = new AsyncCommand(Settings);
            SignOutCommand = new AsyncCommand(SignOut);
            Tips = new List<Tip>()
            {
                new Tip()
                {
                    Title = "Whats the matter?",
                    Description =
                        "The Mafia, also known as the Werewolf, is a group and argumentative game that simulates a battle between a conscious minority and an unconscious majority."
                },
                new Tip()
                {
                    Title = "Ok,What else?",
                    Description =
                        "In general, in this game, the power of speech, maintaining composure and making logical arguments play an important role in victory. Players are secretly identified; Mafias know each other and citizens who are only aware of the number of Mafia members and a few of them are aware of some maps."
                },
                new Tip()
                {
                    Title = "How to play?",
                    Description =
                        "In the night phase of the game, Mafia members secretly kill a citizen. The doctor tries to save the person whom the Mafia wants to kill. The detective also seeks to identify the Mafia, and if he identifies the Mafia, he must prove to other citizens by argument that he is a Mafia. Mafia, doctor, citizen and detective are the main characters of the game and other characters such as sniper may be added to the game in other games. During the day phase, all surviving players discuss mafia identities and vote to remove a suspect."
                },
                new Tip()
                {
                    Title = "How it ends?",
                    Description =
                        "The game continues until all the Mafias are out of the game (citizens win) or the number of Mafias and citizens is equal (Mafia wins) or one of the independent characters, each with a different winning condition, wins the game. In a game, the characters usually have to be arranged in such a way that for each character, there are opposite and complementary characters."
                }
            };
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

            //  await NavigationService.NavigateToPopupAsync<PlayerRoleViewModel>();
        }
    }
}
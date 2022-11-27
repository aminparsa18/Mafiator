using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.GameMembers;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Hubs;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
{
    public class WaitingGameViewModel : ViewModelBase
    {
        //current state of SignalR Game Hub
        private LayoutState currentState = LayoutState.Loading;
        public LayoutState CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        //detail of occuring game
        private AppointedGameResult waiting;

        //total capacity of game
        private int total;
        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        //indicates progress of join to game
        private double capacityPercentage;
        public double CapacityPercentage
        {
            get => capacityPercentage;
            set => SetProperty(ref capacityPercentage, value);
        }

        private bool isJoined = true;
        public bool IsJoined
        {
            get => isJoined;
            set => SetProperty(ref isJoined, value);
        }

        private bool canLeave;
        public bool CanLeave
        {
            get => canLeave;
            set => SetProperty(ref canLeave, value);
        }

        private string leaveText;
        public string LeaveText
        {
            get => leaveText;
            set => SetProperty(ref leaveText, value);
        }

        public IAsyncRelayCommand InviteCommand { get; set; }
        public IAsyncRelayCommand JoinGameCommand { get; set; }
        public IAsyncRelayCommand LeaveGameCommand { get; set; }
        public ObservableRangeCollection<WaitingPlayerResult> Members { get; set; }

        private readonly IGamesApiService _gamesApiService;
        private readonly IGameMemberApiService _gameMemberApiService;

        //to handle hub connection
        private CancellationTokenSource cts;

        public WaitingGameViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
            IGamesApiService gamesApiService, IGameMemberApiService gameMemberApiService) : base(navigationService, localizer, toastService)
        {
            _gamesApiService = gamesApiService;
            _gameMemberApiService = gameMemberApiService;
            Members = new ObservableRangeCollection<WaitingPlayerResult>();
            InviteCommand = new AsyncRelayCommand(Invite);
            JoinGameCommand = new AsyncRelayCommand(JoinGame);
            LeaveGameCommand = new AsyncRelayCommand(LeaveGame);
        }

        private async Task Invite()
        {
            await Share.RequestAsync(new ShareTextRequest()
            {
                Subject = "Join Game",
                Text = "Join Mafiator Game",
                Title = "join",
                Uri = "http://invite.mafiator.com/joinGame/" + waiting.Id
            });
        }

        private async Task LeaveGame()
        {
            if (waiting.Status == GameStatus.Playing)
            {
                await _navigationService.NavigateToAsync<GameViewModel>(waiting.Id.ToString());
                GameHub.Instance.Remove("Join");
                GameHub.Instance.Remove("StartGame");
                return;
            }

            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Leaving Game...");
            var request = await _gamesApiService.LeaveGame(waiting.Id.ToString());
            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsMessagePackAsync<ApiResult>();
                if (response.IsSuccess)
                {
                    CanLeave = false;
                    IsJoined = false;
                    LeaveText = "Leave";
                }
            }
            else
            {
                var response = await request.Content.ReadAsStringAsync();
                _toastService.ShortAlert(response, MessageType.Error);
            }

            await _navigationService.RemovePopupAsync();
        }

        private async Task JoinGame()
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Joining Game...");
            var request = await _gamesApiService.JoinGame(waiting.Id.ToString());
            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsMessagePackAsync<ApiResult>();
                if (response.IsSuccess)
                {
                    CanLeave = true;
                    IsJoined = false;
                    LeaveText = "Leave";
                }
                else
                {
                    _toastService.ShortAlert(string.Join('-', response.Errors), MessageType.Error);
                }
            }
            else
            {
                var response = await request.Content.ReadAsStringAsync();
                _toastService.ShortAlert(response, MessageType.Error);
            }

            await _navigationService.RemovePopupAsync();
        }

        private async void LoadData()
        {
            await Task.WhenAll(StartHub(), GetJoinStatus());
        }

        private async Task StartHub()
        {
            if (GameHub.Instance.State == HubConnectionState.Disconnected)
            {
                try
                {
                    cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                    await GameHub.Instance.StartAsync(cts.Token);
                    CurrentState = LayoutState.Success;
                    GameHub.Instance.Reconnecting += HubReconnecting;
                    GameHub.Instance.Reconnected += HubReconnected;
                    await GameHub.Instance.InvokeAsync("Subscribe", waiting.Id.ToString());
                    GameHub.Instance.On<string>("Join", SomebodyJoined);
                    GameHub.Instance.On<string, string>("StartGame", StartGame);
                }
                catch when (cts.IsCancellationRequested)
                {
                    CurrentState = LayoutState.Error;
                }
            }
        }

        private void StartGame(string ingestUrl, string previewUrl)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                Barrel.Current.Add("IngestUrl", ingestUrl, TimeSpan.FromHours(3));
                Barrel.Current.Add("PreviewUrl", previewUrl, TimeSpan.FromHours(3));
                await _navigationService.NavigateToAsync<GameViewModel>(waiting.Id.ToString());
                GameHub.Instance.Remove("Join");
                GameHub.Instance.Remove("StartGame");
            });
        }

        private async Task SomebodyJoined(string user)
        {
            var members = await _gameMemberApiService.GetWaitingPlayersByGame(waiting.Id.ToString());
            if (members.IsSuccess)
            {
                Members.Clear();
                Members.AddRange(members.Data);
                CapacityPercentage = (double)Members.Count / Total;
            }
        }

        private Task HubReconnected(string arg)
        {
            CurrentState = LayoutState.Success;
            return Task.CompletedTask;
        }

        private Task HubReconnecting(Exception arg)
        {
            CurrentState = LayoutState.Loading;
            return Task.CompletedTask;
        }

        private async Task GetJoinStatus()
        {
            var joinResult = await _gamesApiService.IsGameJoined(waiting.Id.ToString());
            if (joinResult.IsSuccess)
            {
                IsJoined = !string.IsNullOrEmpty(joinResult.Data);
                CanLeave = IsJoined;
                if (IsJoined)
                {
                    if (waiting.Status == GameStatus.Playing)
                        LeaveText = "Play";
                    else if (waiting.Status == GameStatus.NotStarted)
                        leaveText = "Leave";
                }
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is AppointedGameResult waiting)
            {
                this.waiting = waiting;
                Members.AddRange(waiting.Members.Select(w => new WaitingPlayerResult()
                {
                    DisplayName = w.DisplayName,
                    Image = w.Image,
                    Score = w.Score,
                    UserId = w.UserId
                }));
                Total = waiting.Roles.Count;
                CapacityPercentage = (double)Members.Count / Total;
                await Task.Run(LoadData);
            }
            else if (navigationData is Guid gameId)
            {
                await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Loading Game Status...");
                var request = await _gamesApiService.GetAppointedGameDetails(gameId.ToString());
                if (request.IsSuccess)
                {
                    this.waiting = request.Data;
                    Members.AddRange(this.waiting.Members.Select(w => new WaitingPlayerResult()
                    {
                        DisplayName = w.DisplayName,
                        Image = w.Image,
                        Score = w.Score,
                        UserId = w.UserId
                    }));
                    Total = this.waiting.Roles.Count;
                    CapacityPercentage = (double)Members.Count / Total;
                    await Task.Run(LoadData);
                }

                await _navigationService.RemovePopupAsync();
            }
        }

        public void StopHub()
        {
            if (GameHub.Instance.State == HubConnectionState.Connecting)
                cts.Cancel();
            else
                GameHub.Instance.StopAsync(cts.Token);
        }
    }
}
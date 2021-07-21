using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
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
        private WaitingGameDto waiting;

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

        public IAsyncCommand InviteCommand { get; set; }
        public IAsyncCommand JoinGameCommand { get; set; }
        public IAsyncCommand LeaveGameCommand { get; set; }

        //to handle hub connection
        private CancellationTokenSource cts;
        public ObservableRangeCollection<WaitingPlayerDto> Members { get; set; }

        public WaitingGameViewModel()
        {
            Members = new ObservableRangeCollection<WaitingPlayerDto>();
            InviteCommand = new AsyncCommand(Invite);
            JoinGameCommand = new AsyncCommand(JoinGame);
            LeaveGameCommand = new AsyncCommand(LeaveGame);
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
                StartGame();
                return;
            }

            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Leaving Game...");
            var request = await WebApiService.LeaveGame(waiting.Id.ToString());
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
                DependencyService.Get<IAlert>().ShortAlert(response, MessageType.Error);
            }

            await NavigationService.RemovePopupAsync();
        }

        private async Task JoinGame()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Joining Game...");
            var request = await WebApiService.JoinGame(waiting.Id.ToString());
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
                    DependencyService.Get<IAlert>().ShortAlert(string.Join('-', response.Errors), MessageType.Error);
                }
            }
            else
            {
                var response = await request.Content.ReadAsStringAsync();
                DependencyService.Get<IAlert>().ShortAlert(response, MessageType.Error);
            }

            await NavigationService.RemovePopupAsync();
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
                    GameHub.Instance.On("StartGame", StartGame);
                }
                catch when (cts.IsCancellationRequested)
                {
                    CurrentState = LayoutState.Error;
                }
            }
        }

        private void StartGame()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await NavigationService.NavigateToAsync<GameViewModel>(waiting.Id.ToString());
                GameHub.Instance.Remove("Join");
                GameHub.Instance.Remove("StartGame");
            });
        
        }

        private async Task SomebodyJoined(string user)
        {
            var members = await WebApiService.GetWaitingPlayersByGame(waiting.Id.ToString());
            if (members.IsSuccess)
            {
                Members.Clear();
                Members.AddRange(members.Data);
                CapacityPercentage = (double) Members.Count / Total;
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
            var joinResult = await WebApiService.IsGameJoined(waiting.Id.ToString());
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
            if (navigationData is WaitingGameDto waiting)
            {
                this.waiting = waiting;
                Members.AddRange(waiting.Members.Select(w => new WaitingPlayerDto()
                {
                    DisplayName = w.DisplayName,
                    Image = w.Image,
                    Score = w.Score,
                    UserId = w.UserId
                }));
                Total = waiting.Roles.Count;
                CapacityPercentage = (double) Members.Count / Total;
                Task.Run(LoadData);
            }
            else if (navigationData is Ulid gameId)
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Loading Game Status...");
                var request = await WebApiService.GetWaitingGameByGame(gameId.ToString());
                if (request.IsSuccess)
                {
                    this.waiting = request.Data;
                    Members.AddRange(this.waiting.Members.Select(w => new WaitingPlayerDto()
                    {
                        DisplayName = w.DisplayName,
                        Image = w.Image,
                        Score = w.Score,
                        UserId = w.UserId
                    }));
                    Total = this.waiting.Roles.Count;
                    CapacityPercentage = (double) Members.Count / Total;
                    Task.Run(LoadData);
                }

                await NavigationService.RemovePopupAsync();
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
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using MessagePipe;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class MyRoomViewModel : ViewModelBase
    {
        private readonly ISubscriber<UpdateRoomEvent> subscriber;
      public ObservableRangeCollection<RoomMemberDto> Members { get; set; }
        public ObservableRangeCollection<GameRoleDto> Roles { get; set; }

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

        private bool newGameAvailable;

        public bool NewGameAvailable
        {
            get => newGameAvailable;
            set
            {
                newGameAvailable = value;
                RaisePropertyChanged(() => NewGameAvailable);
            }
        }

        private bool isJoined = true;

        public bool IsJoined
        {
            get => isJoined;
            set
            {
                isJoined = value;
                RaisePropertyChanged(() => IsJoined);
            }
        }

        private bool canLeave;

        public bool CanLeave
        {
            get => canLeave;
            set
            {
                canLeave = value;
                RaisePropertyChanged(() => CanLeave);
            }
        }

        private string remainingTime;

        public string RemainingTime
        {
            get => remainingTime;
            set
            {
                remainingTime = value;
                RaisePropertyChanged(() => RemainingTime);
            }
        }

        private LayoutState currentState;

        public LayoutState CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                RaisePropertyChanged(() => CurrentState);
            }
        }

        public IAsyncCommand LoadDataCommand { get; set; }
        public IAsyncCommand AddMemberCommand { get; set; }
        public IAsyncCommand QrCommand { get; set; }
        public IAsyncCommand NewGameCommand { get; set; }
        public IAsyncCommand GoToGameCommand { get; set; }
        public IAsyncCommand JoinCommand { get; set; }
        public IAsyncCommand LeaveCommand { get; set; }
        private ApiResult<WaitingGameDto> waiting;

        public MyRoomViewModel(ISubscriber<UpdateRoomEvent> subscriber)
        {
            this.subscriber = subscriber;
            Members = new ObservableRangeCollection<RoomMemberDto>();
            Roles = new ObservableRangeCollection<GameRoleDto>();
            LoadDataCommand = new AsyncCommand(LoadData);
            AddMemberCommand = new AsyncCommand(AddMember);
            QrCommand = new AsyncCommand(GenerateQr);
            NewGameCommand = new AsyncCommand(StartNewGame);
            GoToGameCommand = new AsyncCommand(GoToGame);
            LeaveCommand = new AsyncCommand(Leave);
            JoinCommand = new AsyncCommand(Join);
            subscriber.Subscribe(async s => await LoadDataCommand.ExecuteAsync());
        }

        private async Task Join()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Joining Room...");
            var request = await WebApiService.JoinRoom(Room.Code);
            if (request.IsSuccessStatusCode)
            {
                var result = await request.Content.ReadAsMessagePackAsync<ApiResult<string>>();
                if (result.IsSuccess)
                {
                    await LoadDataCommand.ExecuteAsync();
                }
                else
                {
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.FirstOrDefault(), MessageType.Error);
                }
            }
            else
            {
                var result = await request.Content.ReadAsStringAsync();
                DependencyService.Get<IAlert>().ShortAlert(result, MessageType.Error);
            }

            await NavigationService.RemovePopupAsync();
        }

        private async Task Leave()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Leaving...");
            var response = await WebApiService.LeaveRoom(Room.Code);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
                if (result.IsSuccess)
                {
                    await LoadDataCommand.ExecuteAsync();
                }
                else
                {
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
                }
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                DependencyService.Get<IAlert>().ShortAlert(result, MessageType.Error);
            }

            await NavigationService.RemovePopupAsync();
        }

        private async Task GoToGame()
        {
            await NavigationService.NavigateToAsync<WaitingGameViewModel>(waiting.Data);
        }

        private async Task StartNewGame()
        {
            await NavigationService.NavigateToPopupAsync<NewGameViewModel>(Room.Id);
        }

        private async Task GenerateQr()
        {
            await Share.RequestAsync(new ShareTextRequest()
            {
                Subject = "Join Room",
                Text = "Join Mafiator Room",
                Title = "join",
                Uri = "http://invite.mafiator.com/joinRoom/" + Room.Id
            });
        }

        private async Task AddMember()
        {
            await NavigationService.NavigateToPopupAsync<NewMemberViewModel>(Room.Id);
        }

        private async Task LoadData()
        {
            CurrentState = LayoutState.Loading;
            var members = await WebApiService.GetMembersByRoom(Room.Id);
            if (members.IsSuccess)
            {
                Members.Clear();
                Members.AddRange(members.Data);
            }

            waiting = await WebApiService.GetWaitingGameByRoom(Room.Id.ToString());
            if (waiting.IsSuccess)
            {
                CurrentState = waiting.Data == null ? LayoutState.Empty : LayoutState.Success;
                if (waiting.Data == null && Room.IsAdmin)
                    NewGameAvailable = true;
                else
                    newGameAvailable = false;
                if (waiting.Data != null)
                {
                    RemainingTime = waiting.Data.Status==GameStatus.NotStarted ? "New game is waiting for members to start":"Game is being played";
                    Roles.AddRange(waiting.Data.Roles.Select(s => new GameRoleDto()
                    {
                        Role = s
                    }));
                }
            }
            else
            {
                CurrentState = LayoutState.Error;
            }

            if (Room.IsAdmin)
            {
                IsJoined = true;
                CanLeave = true;
            }
            else
            {
                var joinResult = await WebApiService.IsRoomJoined(Room.Id.ToString());
                if (joinResult.IsSuccess)
                {
                    IsJoined = !string.IsNullOrEmpty(joinResult.Data);
                    CanLeave = IsJoined;
                }
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is Ulid roomId)
            {
                var response = await WebApiService.GetRoom(roomId.ToString());
                if (response.IsSuccess)
                {
                    response.Data.Id = roomId;
                    Room = response.Data;
                }
                else
                    DependencyService.Get<IAlert>().ShortAlert(response.Errors.ToString(), MessageType.Error);
            }
            else if (navigationData is RoomDto navigatedRoom)
            {
                Room = navigatedRoom;
            }

            await LoadData();
        }
    }
}
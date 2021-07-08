using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MafiatorApp.ViewModels
{
    public class GameEventViewModel : ViewModelBase
    {
        private readonly IPublisher<UpdateMembersEvent> publisher;
        private LayoutState mainState = LayoutState.Empty;
        public LayoutState MainState
        {
            get => mainState;
            set => SetProperty(ref mainState, value);
        }

        private LayoutState sleepState;

        public LayoutState SleepState
        {
            get => sleepState;
            set => SetProperty(ref sleepState, value);
        }

        public ObservableRangeCollection<CandidateDto> Candidates { get; set; }
        public ObservableRangeCollection<GameEventResultDto> Results { get; set; }
        public ObservableRangeCollection<PlayerDto> Partners { get; set; }

        private CandidateDto candidate;

        public CandidateDto Candidate
        {
            get => candidate;
            set => SetProperty(ref candidate, value);
        }

        private Timer _timer;
        private int totalTime;
        private double progressTimer;

        public double ProgressTimer
        {
            get => progressTimer;
            set => SetProperty(ref progressTimer, value);
        }

        private string title = "It's night";

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string subTitle = "play your role";

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }

        private string playerTask;

        public string PlayerTask
        {
            get => playerTask;
            set => SetProperty(ref playerTask, value);
        }

        private string action;
        public string Action
        {
            get => action;
            set => SetProperty(ref action, value);
        }

        private bool isMafia;
        public bool IsMafia
        {
            get => isMafia;
            set => SetProperty(ref isMafia, value);
        }

        public ICommand CandidateSelectedCommand { get; set; }
        public IAsyncCommand ApplyCommand { get; set; }
        private string gameId;
        private readonly IMapper mapper;

        public GameEventViewModel(IMapper mapper,IPublisher<UpdateMembersEvent> publisher)
        {
            this.mapper = mapper;
            this.publisher = publisher;
            Candidates = new ObservableRangeCollection<CandidateDto>();
            Results = new ObservableRangeCollection<GameEventResultDto>();
            Partners = new ObservableRangeCollection<PlayerDto>();
            CandidateSelectedCommand = new Command(CandidateSelected);
            ApplyCommand = new AsyncCommand(Apply);
            _timer ??= new Timer(Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
            GameHub.Instance.On<string>("MafiaChose", MafiaChose);
            GameHub.Instance.On("NightResult", ShowStatus);
        }

        private void MafiaChose(string memberId)
        {
            var members = Barrel.Current.Get<IEnumerable<PlayerDto>>("Members");
            Partners.Add(members.FirstOrDefault(m => m.Id == memberId));
        }

        private void Callback(object state)
        {
            totalTime += 100;
            ProgressTimer = 100 * (double) totalTime / 45000;
            if (totalTime == 45000)
            {
                totalTime = 0;
                _timer?.Dispose();
                _timer = null;
            }
        }

        private void Callback2(object state)
        {
            totalTime += 100;
            ProgressTimer = 100 * (double) totalTime / 20000;
            if (totalTime == 20000)
            {
                totalTime = 0;
                _timer?.Dispose();
                _timer = null;
            }
        }

        private async Task ShowStatus()
        {
            if (_timer != null)
            {
                totalTime = 0;
                await _timer.DisposeAsync();
                _timer = null;
            }

            GameHub.Instance.Remove("NightResult");
            _timer ??= new Timer(Callback2, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
            MainState = LayoutState.Loading;
            var status = await WebApiService.GetEventStatus(gameId);
            if (status.IsSuccess)
            {
                Title = "Status Board";
                SubTitle = "See what happened last night";
                if (status.Data.Any())
                {
                    var players = mapper.Map<List<CandidateDto>>(Barrel.Current.Get<IEnumerable<PlayerDto>>("Members"));
                    var killed = status.Data.FirstOrDefault(e => e.EventType == GameEventType.Killed);
                    var cured = status.Data.FirstOrDefault(e => e.EventType == GameEventType.Cured);
                    var inquired = status.Data.FirstOrDefault(e => e.EventType == GameEventType.Inquired);
                    var silenced = status.Data.FirstOrDefault(e => e.EventType == GameEventType.Silenced);
                    var speak = status.Data.FirstOrDefault(e => e.EventType == GameEventType.Speak);
                    if (killed != null && killed.MemberId != cured?.MemberId)
                        //member id is killed
                        Results.Add(new GameEventResultDto()
                        {
                            MemberId = killed.MemberId,
                            Description = "Can no longer play or vote",
                            Status = "Is Killed",
                            DisplayName = players.FirstOrDefault(p => p.Id == killed.MemberId)?.DisplayName,
                            Image = players.FirstOrDefault(p => p.Id == killed.MemberId)?.Image,
                            EventType = GameEventType.Killed
                        });
                    if (silenced != null)
                        Results.Add(new GameEventResultDto()
                        {
                            MemberId = silenced.MemberId,
                            Description = "Can't Talk tomorrow",
                            Status = "Is Silenced",
                            DisplayName = players.FirstOrDefault(p => p.Id == silenced.MemberId)?.DisplayName,
                            Image = players.FirstOrDefault(p => p.Id == silenced.MemberId)?.Image,
                            EventType = GameEventType.Silenced
                        });
                    if (Results.Any())
                    {
                        Barrel.Current.Add("EventResults", Results.ToList(), TimeSpan.FromMinutes(1));
                       publisher.Publish(new UpdateMembersEvent());// MessagingCenter.Send(this, "UpdateMembers");
                    }
                }

                MainState = LayoutState.Saving;
            }
            else
            {
                MainState = LayoutState.Error;
            }
        }

        private async Task Apply()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Applying Target...");
            var player = Barrel.Current.Get<PlayerRoleDto>("PlayerRole");
            HttpResponseMessage request = null;
            if (player.Role == GameRole.Mafia)
            {
                //sends mafia choice for god father
                await GameHub.Instance.InvokeAsync("MafiaChoice", Candidate.Id);
            }
            else if (player.Role == GameRole.Detective)
            {
                request = await WebApiService.Inquiry(new GameEventDto()
                {
                    MemberId = Ulid.Parse(Candidate.Id),
                    GameId = Ulid.Parse(gameId),
                    EventType = GameEventType.Inquired
                });

                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadAsMessagePackAsync<ApiResult<InquiryStatusDto>>();
                    if (response.IsSuccess)
                    {
                        //show result
                        await NavigationService.NavigateToPopupAsync<InquiryStatusViewModel>(response.Data.IsMafia);
                    }
                    else
                    {
                        DependencyService.Get<IAlert>().ShortAlert(response.Errors.FirstOrDefault(), MessageType.Error);
                    }
                }
                else
                {
                    var response = await request.Content.ReadAsStringAsync();
                    DependencyService.Get<IAlert>().ShortAlert(response, MessageType.Error);
                }
            }
            else if (player.Role == GameRole.Doctor)
            {
                request = await WebApiService.Cure(new GameEventDto()
                {
                    MemberId = Ulid.Parse(Candidate.Id),
                    GameId = Ulid.Parse(gameId),
                    EventType = GameEventType.Cured
                });
            }
            else
            {
                request = await WebApiService.FireGameEvent(new GameEventDto()
                {
                    MemberId = Ulid.Parse(Candidate.Id),
                    GameId = Ulid.Parse(gameId),
                    EventType = GameEventType.Killed
                });
            }

            await NavigationService.RemovePopupAsync();
            if (request != null)
                MainState = request.IsSuccessStatusCode ? LayoutState.Success : LayoutState.Error;
            else
                MainState = LayoutState.Success;
            Title = "Well done";
            SubTitle = "Now wait till morning";
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string data)
            {
                var player = Barrel.Current.Get<PlayerRoleDto>("PlayerRole");
                var members = Barrel.Current.Get<IEnumerable<PlayerDto>>("Members");
                var currentMember = members.FirstOrDefault(m => m.Id == player.MemberId);
                if (player.Role == GameRole.Citizen || player.Role == GameRole.Terrorist)
                {
                    if (currentMember.Status == PlayerStatus.Killed || currentMember.Status == PlayerStatus.Kicked)
                        SleepState = LayoutState.Error;
                    else
                        SleepState = LayoutState.Empty;
                }
                else
                {
                    if (currentMember.Status != PlayerStatus.Kicked && currentMember.Status != PlayerStatus.Killed)
                    {
                        var players = mapper.Map<List<CandidateDto>>(members.Where(m =>
                            m.Status != PlayerStatus.Killed && m.Status != PlayerStatus.Kicked));
                        //players.RemoveAll(p => p.Id == player?.MemberId);
                        gameId = data;
                        Candidates.AddRange(players);
                        PlayerTask = ShowPlayerTask();
                        Action = ShowPlayerAction();
                        if (player.Role == GameRole.GodFather)
                            IsMafia = true;
                    }
                    else
                    {
                        SleepState = LayoutState.Error;
                    }
                }
            }

            return base.InitializeAsync(navigationData);
        }

        private string ShowPlayerTask()
        {
            var player = Barrel.Current.Get<PlayerRoleDto>("PlayerRole");
            switch (player.Role)
            {
                case GameRole.GodFather: return "Kill Someone";
                case GameRole.Mafia: return "Choose Someone to kill";
                case GameRole.Doctor: return "Cure Someone";
                case GameRole.Detective: return "Inquiry Someone";
                case GameRole.Sniper: return "You can shoot or not";
                default: return "Wait for player to take their actions";
            }
        }

        private string ShowPlayerAction()
        {
            var player = Barrel.Current.Get<PlayerRoleDto>("PlayerRole");
            switch (player.Role)
            {
                case GameRole.GodFather: return "Kill";
                case GameRole.Mafia: return "Select";
                case GameRole.Doctor: return "Cure";
                case GameRole.Detective: return "Inquiry";
                case GameRole.Sniper: return "Shoot";
                default: return "Action";
            }
        }

        private void CandidateSelected()
        {
            Candidates.ForEach(c => c.Selected = false);
            Candidate.Selected = !Candidate.Selected;
        }

        public class GameEventResultDto
        {
            public string MemberId { get; set; }
            public string DisplayName { get; set; }
            public string Image { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
            public GameEventType EventType { get; set; }
        }
    }
}
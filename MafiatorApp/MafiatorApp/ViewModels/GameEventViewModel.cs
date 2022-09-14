using AutoMapper;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.GameEvents;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Dtos.GameEvent;
using MafiatorApp.Dtos.Game;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MafiatorApp.ViewModels
{
    public class GameEventViewModel : ViewModelBase
    {
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
        public ObservableRangeCollection<GameEventResult> Results { get; set; }
        public ObservableRangeCollection<PlayerDetails> Partners { get; set; }
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

        private string title = LocalizationResourceManager.Current.GetValue("NightTitle");

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string subTitle = LocalizationResourceManager.Current.GetValue("NightSubTitle");

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

        private readonly IGameEventsApiService _gameEventsApiService;
        private readonly IMapper _mapper;
        private readonly IPublisher<UpdateMembersEvent> _publisher;

        public GameEventViewModel(IGameEventsApiService gameEventsApiService, IMapper mapper, IPublisher<UpdateMembersEvent> publisher)
        {
            _gameEventsApiService = gameEventsApiService;   
            _mapper = mapper;
            _publisher = publisher;
            Candidates = new ObservableRangeCollection<CandidateDto>();
            Results = new ObservableRangeCollection<GameEventResult>();
            Partners = new ObservableRangeCollection<PlayerDetails>();
            CandidateSelectedCommand = new Command(CandidateSelected);
            ApplyCommand = new AsyncCommand(Apply);
            _timer ??= new Timer(Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
            GameHub.Instance.On<string>("MafiaChose", MafiaChose);
            GameHub.Instance.On("NightResult", ShowStatus);
            GameHub.Instance.On<bool>("Inqiry", ShowInquiryResult);
            GameHub.Instance.On<string>("GameFinish", GameFinish);
        }

        private async Task GameFinish(string arg)
        {
            await NavigationService.NavigateToPopupAsync<GameFinishViewModel>(arg);
        }

        private async Task ShowInquiryResult(bool value)
        {
            var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
            if (player.Role == GameRole.Detective)
                await NavigationService.NavigateToPopupAsync<InquiryStatusViewModel>(value);
        }

        private void MafiaChose(string memberId)
        {
            var members = Barrel.Current.Get<IEnumerable<PlayerDetails>>("Members");
            Partners.Add(members.FirstOrDefault(m => m.Id == memberId));
        }

        private void Callback(object state)
        {
            totalTime += 100;
            ProgressTimer = 100 * (double) totalTime / 45000;
            if (totalTime != 45000) 
                return;
            totalTime = 0;
            _timer?.Dispose();
            _timer = null;
        }

        private void Callback2(object state)
        {
            totalTime += 100;
            ProgressTimer = 100 * (double) totalTime / 20000;
            if (totalTime != 20000) 
                return;
            totalTime = 0;
            _timer?.Dispose();
            _timer = null;
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
            var status = await _gameEventsApiService.GetNightResult(gameId);
            if (status.IsSuccess)
            {
                Title = LocalizationResourceManager.Current.GetValue("StatusBoard");
                SubTitle = LocalizationResourceManager.Current.GetValue("StatusBoardSub");
                Results.Clear();
                Results.AddRange(status.Data);
                if (Results.Any())
                {
                    Barrel.Current.Add("EventResults", Results.ToList(), TimeSpan.FromMinutes(1));
                    _publisher.Publish(new UpdateMembersEvent());
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
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Current.GetValue("ApplyingTarget"));
            var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
            HttpResponseMessage request = null;
            if (player.Role == GameRole.Mafia)
            {
                //sends mafia choice for god father
                await GameHub.Instance.InvokeAsync("MafiaChoice", Candidate.Id);
            }
            else if (player.Role == GameRole.Detective)
            {
                request = await _gameEventsApiService.Inquiry(new GameEventRequest()
                {
                    MemberId = Guid.Parse(Candidate.Id),
                    GameId = Guid.Parse(gameId),
                    EventType = GameEventType.Inquired
                });
            }
            else if (player.Role == GameRole.Doctor)
            {
                request = await _gameEventsApiService.Cure(new GameEventRequest()
                {
                    MemberId = Guid.Parse(Candidate.Id),
                    GameId = Guid.Parse(gameId),
                    EventType = GameEventType.Cured
                });
            }
            else
            {
                request = await _gameEventsApiService.FireGameEvent(new GameEventRequest()
                {
                    MemberId = Guid.Parse(Candidate.Id),
                    GameId = Guid.Parse(gameId),
                    EventType = GameEventType.Killed
                });
            }

            await NavigationService.RemovePopupAsync();
            if (request != null)
                MainState = request.IsSuccessStatusCode ? LayoutState.Success : LayoutState.Error;
            else
                MainState = LayoutState.Success;
            Title = LocalizationResourceManager.Current.GetValue("WellDone");
            SubTitle = LocalizationResourceManager.Current.GetValue("NowWait");
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string data)
            {
                var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
                var members = Barrel.Current.Get<IEnumerable<PlayerDetails>>("Members");
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
                        var players = _mapper.Map<List<CandidateDto>>(members.Where(m =>
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

        private static string ShowPlayerTask()
        {
            var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
            return player.Role switch
            {
                GameRole.GodFather => LocalizationResourceManager.Current.GetValue("KillDesc"),
                GameRole.Mafia => LocalizationResourceManager.Current.GetValue("MafiaDesc"),
                GameRole.Doctor => LocalizationResourceManager.Current.GetValue("CureDesc"),
                GameRole.Detective => LocalizationResourceManager.Current.GetValue("InquiryDesc"),
                GameRole.Sniper => LocalizationResourceManager.Current.GetValue("ShootDesc"),
                GameRole.Natasha => LocalizationResourceManager.Current.GetValue("SilenceDesc"),
                GameRole.Priest => LocalizationResourceManager.Current.GetValue("GiveSpeechDesc"),
                _ => LocalizationResourceManager.Current.GetValue("DefaultActionDesc")
            };
        }

        private static string ShowPlayerAction()
        {
            var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
            return player.Role switch
            {
                GameRole.GodFather => LocalizationResourceManager.Current.GetValue("Kill"),
                GameRole.Mafia => LocalizationResourceManager.Current.GetValue("Select"),
                GameRole.Doctor => LocalizationResourceManager.Current.GetValue("Cure"),
                GameRole.Detective => LocalizationResourceManager.Current.GetValue("Inquiry"),
                GameRole.Sniper => LocalizationResourceManager.Current.GetValue("Shoot"),
                GameRole.Natasha => LocalizationResourceManager.Current.GetValue("Silence"),
                GameRole.Priest => LocalizationResourceManager.Current.GetValue("GiveSpeech"),
                _ => LocalizationResourceManager.Current.GetValue("Action")
            };
        }

        private void CandidateSelected()
        {
            Candidates.ForEach(c => c.Selected = false);
            Candidate.Selected = !Candidate.Selected;
        }
    }
}
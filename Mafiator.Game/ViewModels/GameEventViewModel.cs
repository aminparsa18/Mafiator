using AutoMapper;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.ChatMessages;
using Mafiator.Common.Client.Services.GameEvents;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Game.Hubs;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Input;

namespace Mafiator.Game.ViewModels;

public partial class GameEventViewModel : ViewModelBase
{
    private Timer _timer;
    private int totalTime;
    private string gameId;

    private readonly IGameEventsApiService _gameEventsApiService;
    private readonly IMapper _mapper;
    private readonly IPublisher<UpdateMembersEvent> _publisher;

    [ObservableProperty]
    private LayoutState mainState = LayoutState.Empty;

    [ObservableProperty]
    private LayoutState sleepState;

    [ObservableProperty]
    private CandidateDto candidate;

    [ObservableProperty]
    private double progressTimer;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string subTitle;

    [ObservableProperty]
    private string playerTask;

    [ObservableProperty]
    private string action;

    [ObservableProperty]
    private bool isMafia;

    public ObservableRangeCollection<CandidateDto> Candidates { get; set; }
    public ObservableRangeCollection<GameEventResult> Results { get; set; }
    public ObservableRangeCollection<PlayerDetails> Partners { get; set; }
    public ICommand CandidateSelectedCommand { get; set; }
    public IAsyncRelayCommand ApplyCommand { get; set; }

    public GameEventViewModel(INavigationService navigationService, IToastService toastService, IChatMessagesApiService chatMessagesApiService,
        IGameEventsApiService gameEventsApiService, IMapper mapper, IPublisher<UpdateMembersEvent> publisher) 
        :base(navigationService, toastService)
    {
        _gameEventsApiService = gameEventsApiService;
        _mapper = mapper;
        _publisher = publisher;
        Candidates = new ObservableRangeCollection<CandidateDto>();
        Results = new ObservableRangeCollection<GameEventResult>();
        Partners = new ObservableRangeCollection<PlayerDetails>();
        Title = LocalizationResourceManager.Instance["NightTitle"];
        SubTitle = LocalizationResourceManager.Instance["NightSubTitle"];
        CandidateSelectedCommand = new Command(CandidateSelected);
        ApplyCommand = new AsyncRelayCommand(Apply);
        _timer ??= new Timer(Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
        GameHub.Instance.On<string>("MafiaChose", MafiaChose);
        GameHub.Instance.On("NightResult", ShowStatus);
        GameHub.Instance.On<bool>("Inqiry", ShowInquiryResult);
        GameHub.Instance.On<string>("GameFinish", GameFinish);
    }

    private async Task GameFinish(string arg)
    {
        await _navigationService.NavigateToPopupAsync<GameFinishViewModel>(arg);
    }

    private async Task ShowInquiryResult(bool value)
    {
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        if (player.Role == GameRole.Detective)
            await _navigationService.NavigateToPopupAsync<InquiryStatusViewModel>(value);
    }

    private void MafiaChose(string memberId)
    {
        var members = Barrel.Current.Get<IEnumerable<PlayerDetails>>("Members");
        Partners.Add(members.FirstOrDefault(m => m.Id == memberId));
    }

    private void Callback(object state)
    {
        totalTime += 100;
        ProgressTimer = 100 * (double)totalTime / 45000;
        if (totalTime != 45000)
            return;
        totalTime = 0;
        _timer?.Dispose();
        _timer = null;
    }

    private void Callback2(object state)
    {
        totalTime += 100;
        ProgressTimer = 100 * (double)totalTime / 20000;
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
            Title = LocalizationResourceManager.Instance["StatusBoard"];
            SubTitle = LocalizationResourceManager.Instance["StatusBoardSub"];
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
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Instance["ApplyingTarget"]);
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        HttpResponseMessage request = null;
        if (player.Role == GameRole.Mafia)
            //sends mafia choice for god father
            await GameHub.Instance.InvokeAsync("MafiaChoice", Candidate.Id);
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

        await _navigationService.RemovePopupAsync();
        if (request != null)
            MainState = request.IsSuccessStatusCode ? LayoutState.Success : LayoutState.Error;
        else
            MainState = LayoutState.Success;
        Title = LocalizationResourceManager.Instance["WellDone"];
        SubTitle = LocalizationResourceManager.Instance["NowWait"];
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

    private string ShowPlayerTask()
    {
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        return player.Role switch
        {
            GameRole.GodFather => LocalizationResourceManager.Instance["KillDesc"],
            GameRole.Mafia => LocalizationResourceManager.Instance["MafiaDesc"],
            GameRole.Doctor => LocalizationResourceManager.Instance["CureDesc"],
            GameRole.Detective => LocalizationResourceManager.Instance["InquiryDesc"],
            GameRole.Sniper => LocalizationResourceManager.Instance["ShootDesc"],
            GameRole.Natasha => LocalizationResourceManager.Instance["SilenceDesc"],
            GameRole.Priest => LocalizationResourceManager.Instance["GiveSpeechDesc"],
            _ => LocalizationResourceManager.Instance["DefaultActionDesc"]
        };
    }

    private string ShowPlayerAction()
    {
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        return player.Role switch
        {
            GameRole.GodFather => LocalizationResourceManager.Instance["Kill"],
            GameRole.Mafia => LocalizationResourceManager.Instance["Select"],
            GameRole.Doctor => LocalizationResourceManager.Instance["Cure"],
            GameRole.Detective => LocalizationResourceManager.Instance["Inquiry"],
            GameRole.Sniper => LocalizationResourceManager.Instance["Shoot"],
            GameRole.Natasha => LocalizationResourceManager.Instance["Silence"],
            GameRole.Priest => LocalizationResourceManager.Instance["GiveSpeech"],
            _ => LocalizationResourceManager.Instance["Action"]
        };
    }

    private void CandidateSelected()
    {
        foreach (var candidate in Candidates)
        {
            candidate.Selected = false;
        }
        Candidate.Selected = !Candidate.Selected;
    }
}
using AutoMapper;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.Votes;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Enums;
using Mafiator.Game.Hubs;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Models.Vote;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;
using System.Windows.Input;

namespace Mafiator.Game.ViewModels;

public class CandidatesViewModel : ViewModelBase
{
    private LayoutState _currentState = LayoutState.Empty;
    public LayoutState CurrentState
    {
        get => _currentState;
        set => SetProperty(ref _currentState, value);
    }

    private string _title;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _subTitle;
    public string SubTitle
    {
        get => _subTitle;
        set => SetProperty(ref _subTitle, value);
    }



    private CandidateDto _candidate;
    public CandidateDto Candidate
    {
        get => _candidate;
        set => SetProperty(ref _candidate, value);
    }

    private double _progressTimer;
    public double ProgressTimer
    {
        get => _progressTimer;
        set => SetProperty(ref _progressTimer, value);
    }

    public ObservableRangeCollection<CandidateDto> Candidates { get; set; }
    public ObservableRangeCollection<VoteStatusResultDto> Votes { get; set; }
    public ICommand CandidateSelectedCommand { get; set; }
    public IAsyncRelayCommand SendVotesCommand { get; set; }

    private Timer _timer;
    private int _totalTime;
    private bool _advocacy;
    private string _gameId;

    private readonly IMapper _mapper;
    private readonly IPublisher<UpdateMembersEvent> _publisher;
    private readonly IVotesApiService _votesApiService;


    public CandidatesViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService,
        IMapper mapper, IPublisher<UpdateMembersEvent> publisher, IVotesApiService votesApiService) : base(navigationService, localizer, toastService)
    {
        _mapper = mapper;
        _publisher = publisher;
        _votesApiService = votesApiService;
        Title = _localizer["VotingTitle"];
        SubTitle = _localizer["VotingSubTitle"];
        Candidates = new ObservableRangeCollection<CandidateDto>();
        Votes = new ObservableRangeCollection<VoteStatusResultDto>();
        CandidateSelectedCommand = new Command(CandidateSelected);
        SendVotesCommand = new AsyncRelayCommand(SendVotes);
        _timer ??= new Timer(Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
        GameHub.Instance.On("ShowVoteStatus", ShowStatus);
        GameHub.Instance.On("Night", FirstNight);
    }

    private async Task FirstNight()
    {
        GameHub.Instance.Remove("Night");
        await Task.WhenAll(_navigationService.RemovePopupAsync(),
            _navigationService.NavigateToPopupAsync<GameEventViewModel>(_gameId));
    }

    private void Callback(object state)
    {
        _totalTime += 100;
        ProgressTimer = 100 * (double)_totalTime / 45000;
        if (_totalTime != 45000)
            return;
        _totalTime = 0;
        _timer?.Dispose();
        _timer = null;
    }

    private void Callback2(object state)
    {
        _totalTime += 100;
        ProgressTimer = 100 * (double)_totalTime / 20000;
        if (_totalTime != 20000)
            return;
        _totalTime = 0;
        _timer?.Dispose();
        _timer = null;
    }

    private async Task ShowStatus()
    {
        if (_timer != null)
        {
            _totalTime = 0;
            await _timer.DisposeAsync();
            _timer = null;
        }

        _timer ??= new Timer(Callback2, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
        CurrentState = LayoutState.Loading;
        var votesStatus = await _votesApiService.GetVotesStatus(_gameId);
        if (votesStatus.IsSuccess)
        {
            var members = Barrel.Current.Get<IEnumerable<PlayerDetails>>("Members");
            var votes = new List<VoteStatusResultDto>();
            foreach (var member in members)
            {
                var vote = new VoteStatusResultDto()
                {
                    MemberId = member.Id,
                    Image = member.Image,
                    DisplayName = member.DisplayName,
                };
                var voters = votesStatus.Data.Where(v => v.TargetId == member.Id).Select(s => s.VoterId);
                foreach (var voter in voters)
                {
                    vote.Votes += string.Join("", members.FirstOrDefault(m => m.Id == voter)?.DisplayName, " - ");
                }
                vote.Voters = new List<string>();
                foreach (var voter in voters)
                {
                    vote.Voters.Add(members.FirstOrDefault(m => m.Id == voter).Id);
                }
                votes.Add(vote);
            }
            if (_advocacy)
            {
                var candidates = Barrel.Current.Get<IEnumerable<string>>("AdvocacyCandidates");
                votes.RemoveAll(v => !candidates.Contains(v.MemberId));
                Votes.AddRange(votes.OrderByDescending(v => v.Voters.Count));
                var finalCandidates = votesStatus.Data.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                    .Select(grp => grp.Key).Take(2);
                foreach (var candidate in finalCandidates)
                {
                    Votes.FirstOrDefault(f => f.MemberId == candidate).Status = CandidateStatus.Kicked;
                }
                _publisher.Publish(new UpdateMembersEvent());//  MessagingCenter.Send(this,"UpdateMembers");
            }
            else
            {
                Votes.AddRange(votes.OrderByDescending(v => v.Voters.Count));
                var candidates = votesStatus.Data.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                    .Select(grp => grp.Key).Take(2);
                if (candidates.Any())
                    Barrel.Current.Add("AdvocacyCandidates", candidates, TimeSpan.FromHours(2));
                foreach (var candidate in candidates)
                {
                    Votes.FirstOrDefault(f => f.MemberId == candidate).Status = CandidateStatus.Advocacy;
                }

            }
            Title = _localizer["VotingResultTitle"];
            SubTitle = _localizer["VotingResultSubTitle"];
            CurrentState = LayoutState.Saving;
        }
        else
            CurrentState = LayoutState.Error;

        GameHub.Instance.Remove("ShowVoteStatus");
    }


    private async Task SendVotes()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>(_localizer["SendingVotes"]);
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        var request = await _votesApiService.SendVotes(new VoteCreateRequest()
        {
            Targets = Candidates.Where(c => c.Selected).Select(s => Guid.Parse(s.Id)).ToList(),
            VoterId = Guid.Parse(player.MemberId),
            GameId = Guid.Parse(_gameId)
        });
        CurrentState = request.IsSuccessStatusCode ? LayoutState.Success : LayoutState.Error;
        Title = _localizer["VoteSentTitle"];
        SubTitle = _localizer["VoteSentSubTitle"];
        await _navigationService.RemovePopupAsync();
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is not Tuple<List<PlayerDetails>, string, bool> data)
            return base.InitializeAsync(navigationData);
        var player = Barrel.Current.Get<PlayerRoleResult>("PlayerRole");
        var players = _mapper.Map<List<CandidateDto>>(data.Item1.Where(p => p.Status != PlayerStatus.Killed && p.Status != PlayerStatus.Kicked).ToList());
        players.RemoveAll(p => p.Id == player?.MemberId);
        _gameId = data.Item2;
        _advocacy = data.Item3;
        // If its advocay voting just vote previous candidates.
        if (data.Item3)
        {
            var candidates = Barrel.Current.Get<IEnumerable<string>>("AdvocacyCandidates");
            Candidates.AddRange(players.Where(p => candidates.Contains(p.Id)));
        }
        else
            Candidates.AddRange(players);

        return base.InitializeAsync(navigationData);
    }

    private void CandidateSelected()
    {
        if (Candidate == null) return;
        Candidate.Selected = !Candidate.Selected;
        Candidate = null;
    }
}
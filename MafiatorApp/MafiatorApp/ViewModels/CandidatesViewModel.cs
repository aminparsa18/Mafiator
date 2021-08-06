using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MafiatorApp.ViewModels
{
    public class CandidatesViewModel : ViewModelBase
    {
        private readonly IPublisher<UpdateMembersEvent> publisher;

        private LayoutState currentState = LayoutState.Empty;
        public LayoutState CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        private string title= LocalizationResourceManager.Current.GetValue("VotingTitle");

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private string subTitle = LocalizationResourceManager.Current.GetValue("VotingSubTitle");
        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }
        public ObservableRangeCollection<CandidateDto> Candidates { get; set; }
        public ObservableRangeCollection<VoteStatusResultDto> Votes { get; set; }
        private CandidateDto candidate;

        public CandidateDto Candidate
        {
            get => candidate;
            set => SetProperty(ref candidate, value);
        }

        private Timer _timer;
        private int totalTime;
        private double progressTimer;
        private bool advocacy;
        public double ProgressTimer
        {
            get => progressTimer;
            set => SetProperty(ref progressTimer, value);
        }


        public ICommand CandidateSelectedCommand { get; set; }
        public IAsyncCommand SendVotesCommand { get; set; }
        private string gameId;
        private readonly IMapper mapper;

        public CandidatesViewModel(IMapper mapper,IPublisher<UpdateMembersEvent> publisher)
        {
            this.mapper = mapper;
            this.publisher = publisher;
            Candidates = new ObservableRangeCollection<CandidateDto>();
            Votes = new ObservableRangeCollection<VoteStatusResultDto>();
            CandidateSelectedCommand = new Command(CandidateSelected);
            SendVotesCommand = new AsyncCommand(SendVotes);
            _timer ??= new Timer(Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
            GameHub.Instance.On("ShowVoteStatus", ShowStatus);
            GameHub.Instance.On("Night", FirstNight);
        }

        private async Task FirstNight()
        {
            GameHub.Instance.Remove("Night");
            await Task.WhenAll(NavigationService.RemovePopupAsync(),
                NavigationService.NavigateToPopupAsync<GameEventViewModel>(gameId));
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

            _timer ??= new Timer(Callback2, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
            CurrentState = LayoutState.Loading;
            var votesStatus = await WebApiService.GetVotesStatus(gameId);
            if (votesStatus.IsSuccess)
            {
                var members = Barrel.Current.Get<IEnumerable<PlayerDto>>("Members");
                var votes=new List<VoteStatusResultDto>();
                foreach (var member in members)
                {
                    var vote = new VoteStatusResultDto()
                    {
                        MemberId = member.Id,
                        Image = member.Image,
                        DisplayName = member.DisplayName,
                    };
                    var voters = votesStatus.Data.Where(v => v.TargetId == member.Id).Select(s => s.VoterId);
                    voters.ForEach(v => vote.Votes += members.FirstOrDefault(m => m.Id == v)?.DisplayName + " - ");
                    vote.Voters=new List<string>();
                    voters.ForEach(v => vote.Voters.Add(members.FirstOrDefault(m => m.Id == v).Id));
                    votes.Add(vote);
                }
                if (advocacy)
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
                    publisher.Publish(new UpdateMembersEvent());//  MessagingCenter.Send(this,"UpdateMembers");
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
                Title = LocalizationResourceManager.Current.GetValue("VotingResultTitle");
                SubTitle = LocalizationResourceManager.Current.GetValue("VotingResultSubTitle");
                CurrentState = LayoutState.Saving;
            }
            else
            {
                CurrentState = LayoutState.Error;
            }

            GameHub.Instance.Remove("ShowVoteStatus");
        }


        private async Task SendVotes()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Current.GetValue("SendingVotes"));
            var player = Barrel.Current.Get<PlayerRoleDto>("PlayerRole");
            var request = await WebApiService.SendVotes(new VoteDto()
            {
                Targets = Candidates.Where(c => c.Selected).Select(s => Ulid.Parse(s.Id)).ToList(),
                VoterId = Ulid.Parse(player.MemberId),
                GameId = Ulid.Parse(gameId)
            });
            if (request.IsSuccessStatusCode)
            {
                CurrentState = LayoutState.Success;
            }
            else
            {
                CurrentState = LayoutState.Error;
            }
            Title = LocalizationResourceManager.Current.GetValue("VoteSentTitle");
            SubTitle = LocalizationResourceManager.Current.GetValue("VoteSentSubTitle");
            await NavigationService.RemovePopupAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Tuple<List<PlayerDto>, string, bool> data)
            {
                var player = Barrel.Current.Get<PlayerRoleDto>("PlayerRole");
                var players = mapper.Map<List<CandidateDto>>(data.Item1.Where(p=>p.Status!=PlayerStatus.Killed && p.Status!=PlayerStatus.Kicked).ToList());
                players.RemoveAll(p => p.Id == player?.MemberId);
                gameId = data.Item2;
                advocacy = data.Item3;
                //if its advocay voting just vote previous candidates
                if (data.Item3)
                {
                    var candidates = Barrel.Current.Get<IEnumerable<string>>("AdvocacyCandidates");
                    Candidates.AddRange(players.Where(p => candidates.Contains(p.Id)));
                }
                else
                    Candidates.AddRange(players);
            }

            return base.InitializeAsync(navigationData);
        }

        private void CandidateSelected()
        {
            if (Candidate == null) return;
            Candidate.Selected = !Candidate.Selected;
            Candidate = null;
        }

    }
}
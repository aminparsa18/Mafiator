using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Mafiator.Entities.Enums;
using Mafiator.IocConfig.Hubs;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.SignalR;
using RepoDb.Exceptions;

namespace Mafiator.Api
{
    public class GameService : IGameService
    {
        //distributed Cache
        private readonly IMemoryCache cache;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<GameHub> _gameHub;

        public GameService(IMemoryCache cache, IUnitOfWork unitOfWork, IHubContext<GameHub> gameHub)
        {
            this.cache = cache;
            this.unitOfWork = unitOfWork;
            this._gameHub = gameHub;
        }

        //turn change every 45 seconds
        public async Task SetTurn(string gameId, int index)
        {
            var _members = await unitOfWork.GameMember.GetPlayerByGame(gameId);
            await _gameHub.Clients.Group(gameId).SendAsync("Turn", _members.ElementAt(index).MemberId);
            index++;
            string jobId;
            //reach last player in group
            if (index == _members.Count())
            {
                //wait 45 second until last mother fucker talks then start voting
               jobId= BackgroundJob.Schedule(() => SendTargets(gameId), TimeSpan.FromSeconds(45));
            }
            else
            {
               jobId = BackgroundJob.Schedule(() => SetTurn(gameId, index), TimeSpan.FromSeconds(45));
            }
            cache.SetCache(jobId, "JobId" + gameId);
        }

        //sends list of targets to each user
        public async Task SendTargets(string gameId)
        {
            await _gameHub.Clients.Group(gameId).SendAsync("ShowCandidates");
            BackgroundJob.Schedule(() => ShowVoteResult(gameId), TimeSpan.FromSeconds(45));
        }

        //send appropriate target for each role
        //for example present list of citizens to mafia and they select victim
        public async Task ShowVoteResult(string gameId)
        {
            await _gameHub.Clients.Group(gameId).SendAsync("ShowVoteStatus");
            BackgroundJob.Schedule(() => InitVotes(gameId), TimeSpan.FromSeconds(20));
        }

        public async Task InitVotes(string gameId)
        {
            var votes = await unitOfWork.Vote.GetNonValidatedTargets(gameId);
            // take the most voted to advocacy
            var candidates = votes.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                .Select(grp => grp.Key).Take(2);
            if (candidates.Any())
            {
                BackgroundJob.Enqueue(() => SetAdvocacyTurn(gameId, 0));
            }
            else
            {
                CacheFactory.GetCache().Remove($"Targets-{gameId}");
                BackgroundJob.Enqueue(() => Night(gameId));
            }
        }

        //turn change every 45 seconds
        public async Task SetAdvocacyTurn(string gameId, int index)
        {
            var votes = await unitOfWork.Vote.GetNonValidatedTargets(gameId);
            // take the most voted to advocacy
            var candidates = votes.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                .Select(grp => grp.Key).Take(2);
            await _gameHub.Clients.Group(gameId).SendAsync("Turn", candidates.ElementAt(index));
            index++;
            string jobId;
            //reach last player in advocacy
            if (index == candidates.Count())
            {
                //wait 45 second until last mother fucker talks then start voting
                 jobId = BackgroundJob.Schedule(() => SendAdvocacyTargets(gameId), TimeSpan.FromSeconds(45));
                //cache candidates for second voting
                cache.SetCache(candidates, $"Targets-{gameId}");
                await unitOfWork.Vote.Validate(gameId);
                CacheFactory.GetCache().Remove($"Targets-{gameId}");
            }
            else
            {
                 jobId = BackgroundJob.Schedule(() => SetAdvocacyTurn(gameId, index), TimeSpan.FromSeconds(45));
            }
            cache.SetCache(jobId, "JobId"+gameId);
        }
    
        //sends list of target to each user
        public async Task SendAdvocacyTargets(string gameId)
        {
            //validate votes so it changes in next days
            await _gameHub.Clients.Group(gameId).SendAsync("ShowAdvocacyCandidates");
            BackgroundJob.Schedule(() => FinalizeVote(gameId), TimeSpan.FromSeconds(45));
        }

        //send appropriate target for each role
        //for example present list of citizens to mafia and they select victim
        public async Task FinalizeVote(string gameId)
        {
            var votes = await unitOfWork.Vote.GetNonValidatedTargets(gameId);
            // take the most voted to advocacy
            var candidates = votes.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                .Select(grp => grp.Key).Take(2);
            if (candidates.Any())
            {
                foreach (var candidate in candidates)
                {
                    var statuses= await unitOfWork.GameMember.GetPlayerStatusFast(candidate);
                    var status = statuses.FirstOrDefault();
                    await unitOfWork.GameMember.KickMember(candidate);
                    if(status.GameRole==GameRole.Terrorist)
                        await _gameHub.Clients.Group(gameId).SendAsync("KickWithTerrorist");
                }
                //remove cache of stored players due to status change
                CacheFactory.GetCache().Remove($"GamePlayers-{gameId}");
            }
            await _gameHub.Clients.Group(gameId).SendAsync("ShowVoteStatus");
            BackgroundJob.Schedule(() => Night(gameId), TimeSpan.FromSeconds(30));
        }

        public async Task Night(string gameId)
        {
            await unitOfWork.Vote.Validate(gameId);
            await _gameHub.Clients.Group(gameId).SendAsync("Night");
            BackgroundJob.Schedule(() => ShowNightResult(gameId), TimeSpan.FromSeconds(45));

        }

        public async Task ShowNightResult(string gameId)
        {
            var events = await unitOfWork.GameEvent.GetByGame(gameId);
            var _members = await unitOfWork.GameMember.GetPlayerByGame(gameId);
            if (events.Any())
            {
                var killed = events.FirstOrDefault(e => e.EventType == GameEventType.Killed);
                var cured = events.FirstOrDefault(e => e.EventType == GameEventType.Cured);
                var inquired = events.FirstOrDefault(e => e.EventType == GameEventType.Inquired);
                var silenced = events.FirstOrDefault(e => e.EventType == GameEventType.Silenced);
                var speak = events.FirstOrDefault(e => e.EventType == GameEventType.Speak);
                var sniped = events.FirstOrDefault(e => e.EventType == GameEventType.Sniped);
                var poisoned = events.FirstOrDefault(e => e.EventType == GameEventType.Poisoned);
                //i think it has priority over all
                if (poisoned != null)
                {
                    var fuckedUp = _members.FirstOrDefault(m => m.MemberId == poisoned.MemberId);
                    if (fuckedUp.Role == GameRole.GodFather)
                        killed = null;
                    else if(fuckedUp.Role==GameRole.Sniper)
                    {
                        sniped = null;
                    }else if (fuckedUp.Role == GameRole.Natasha)
                    {
                        silenced = null;
                    }else if (fuckedUp.Role == GameRole.Priest)
                    {
                        speak = null;
                    }
                }

                if (killed != null && killed.MemberId != cured?.MemberId)
                {
                    var fuckedUp = _members.FirstOrDefault(m => m.MemberId == killed.MemberId);
                    if(fuckedUp?.Role!=GameRole.Immortal)
                        await unitOfWork.GameMember.KillMember(killed.MemberId);
                }

                if (speak != null)
                {
                    if (speak.MemberId == silenced?.MemberId)
                        silenced = null;
                }

                if (silenced != null)
                    await unitOfWork.GameMember.SilenceMember(silenced.MemberId);
                if (sniped != null)
                {
                    var targets = await unitOfWork.GameMember.GetPlayerStatusFast(sniped.MemberId);
                    if (targets.Any())
                    {
                        var target = targets.FirstOrDefault();
                        //if sniper shoots a mafia he will be killed himself
                        if (target.GameRole == GameRole.GodFather || target.GameRole == GameRole.Mafia ||
                            target.GameRole == GameRole.Terrorist)
                        {
                            var snipers = await unitOfWork.GameMember.GetPlayerByRoleFast(gameId,(short)GameRole.Sniper);
                            if (snipers.Any())
                            {
                                var sniper = snipers.FirstOrDefault();
                                await unitOfWork.GameMember.KillMember(sniper.MemberId);
                            }
                        }
                        else
                        {
                            await unitOfWork.GameMember.KillMember(sniped.MemberId);
                        }

                    }
                }

                try
                {
                    //remove player status cache
                    CacheFactory.GetCache().Remove($"GamePlayers-{gameId}");
                }
                catch(ItemNotFoundException){}
            }
            await _gameHub.Clients.Group(gameId).SendAsync("NightResult");
            BackgroundJob.Schedule(() => ValidateGameEvents(gameId), TimeSpan.FromSeconds(20));

        }

        public async Task ValidateGameEvents(string gameId)
        {
            await unitOfWork.GameEvent.Validate(gameId);
            BackgroundJob.Enqueue(() => SetTurn(gameId, 0));

        }
    }
}
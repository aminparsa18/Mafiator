using Hangfire;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Data.Extensions;
using Mafiator.IocConfig.Hubs;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.SignalR;
using RepoDb.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Api
{
    public class GameService : IGameService
    {
        //distributed Cache
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<GameHub> _gameHub;

        public GameService(IMemoryCache cache, IUnitOfWork unitOfWork, IHubContext<GameHub> gameHub)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _gameHub = gameHub;
        }

        //turn change every 45 seconds
        public async Task SetTurn(string gameId, int index)
        {
            var members = await _unitOfWork.GameMember.GetPlayerByGame(gameId);
            await _gameHub.Clients.Group(gameId).SendAsync("Turn", members.ElementAt(index).MemberId);
            index++;
            string jobId;
            //reach last player in group
            if (index == members.Count())
            {
                //wait 45 second until last mother fucker talks then start voting
                jobId = BackgroundJob.Schedule(() => SendTargets(gameId), TimeSpan.FromSeconds(30));
            }
            else
            {
                jobId = BackgroundJob.Schedule(() => SetTurn(gameId, index), TimeSpan.FromSeconds(30));
            }

            _cache.SetCache(jobId, "JobId" + gameId);
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
            var votes = await _unitOfWork.Vote.GetNonValidatedTargets(gameId);
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
            var votes = await _unitOfWork.Vote.GetNonValidatedTargets(gameId);
            // take the most voted to advocacy
            var candidates = votes.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                .Select(grp => grp.Key).Take(2);
            await _gameHub.Clients.Group(gameId).SendAsync("AdvocacyTurn", candidates.ElementAt(index));
            index++;
            string jobId;
            //reach last player in advocacy
            if (index == candidates.Count())
            {
                //wait 45 second until last mother fucker talks then start voting
                jobId = BackgroundJob.Schedule(() => SendAdvocacyTargets(gameId), TimeSpan.FromSeconds(45));
                //cache candidates for second voting
                _cache.SetCache(candidates, $"Targets-{gameId}");
                await _unitOfWork.Vote.Validate(gameId);
                CacheFactory.GetCache().Remove($"Targets-{gameId}");
            }
            else
            {
                jobId = BackgroundJob.Schedule(() => SetAdvocacyTurn(gameId, index), TimeSpan.FromSeconds(45));
            }

            _cache.SetCache(jobId, "JobId" + gameId);
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
            var finish = false;
            var votes = await _unitOfWork.Vote.GetNonValidatedTargets(gameId);
            // take the most voted to advocacy
            var candidates = votes.GroupBy(i => i.TargetId).OrderByDescending(s => s.Count())
                .Select(grp => grp.Key).Take(2);
            if (candidates.Any())
            {
                foreach (var candidate in candidates)
                {
                    var statuses = await _unitOfWork.GameMember.GetPlayerStatusFast(candidate);
                    var status = statuses.FirstOrDefault();
                    await _unitOfWork.GameMember.KickMember(candidate);
                    if (status.GameRole == GameRole.Terrorist)
                        await _gameHub.Clients.Group(gameId).SendAsync("KickWithTerrorist");
                }

                //remove cache of stored players due to status change
                CacheFactory.GetCache().Remove($"GamePlayers-{gameId}");
            }

            await _gameHub.Clients.Group(gameId).SendAsync("ShowVoteStatus");
            if (candidates.Any())
                finish = await CheckFinish(gameId);
            if (!finish)
                BackgroundJob.Schedule(() => Night(gameId), TimeSpan.FromSeconds(30));
        }

        public async Task Night(string gameId)
        {
            await _unitOfWork.Vote.Validate(gameId);
            await _gameHub.Clients.Group(gameId).SendAsync("Night");
            BackgroundJob.Schedule(() => ShowNightResult(gameId), TimeSpan.FromSeconds(45));
        }

        public async Task ShowNightResult(string gameId)
        {
            var finish = false;
            var results = new List<GameEventResult>();
            var events = await _unitOfWork.GameEvent.GetByGame(gameId);
            var members = await _unitOfWork.GameMember.GetPlayerByGame(gameId);
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
                    var fuckedUp = members.FirstOrDefault(m => m.MemberId == poisoned.MemberId);
                    if (fuckedUp.Role == GameRole.GodFather)
                        killed = null;
                    else if (fuckedUp.Role == GameRole.Sniper)
                    {
                        sniped = null;
                    }
                    else if (fuckedUp.Role == GameRole.Natasha)
                    {
                        silenced = null;
                    }
                    else if (fuckedUp.Role == GameRole.Priest)
                    {
                        speak = null;
                    }
                    else if (fuckedUp.Role == GameRole.Doctor)
                    {
                        cured = null;
                    }


                    if (fuckedUp.Role == GameRole.Detective && inquired != null)
                    {
                        Inquiry(gameId, inquired.MemberId, true);
                    }
                    else if (inquired != null)
                    {
                        Inquiry(gameId, inquired.MemberId, false);
                    }
                }

                if (killed != null && killed.MemberId != cured?.MemberId)
                {
                    var fuckedUp = members.FirstOrDefault(m => m.MemberId == killed.MemberId);
                    if (fuckedUp?.Role != GameRole.Immortal)
                        await _unitOfWork.GameMember.KillMember(killed.MemberId);
                    results.Add(new GameEventResult()
                    {
                        MemberId = killed.MemberId,
                        //Description = "Can no longer play or vote",
                        //Status = "Is Killed",
                        //DisplayName = _members.FirstOrDefault(p => p.MemberId == killed.MemberId)?.UserId,
                        EventType = GameEventType.Killed
                    });
                }

                if (speak != null)
                {
                    if (speak.MemberId == silenced?.MemberId)
                        silenced = null;
                    else
                    {
                        results.Add(new GameEventResult()
                        {
                            MemberId = speak.MemberId,
                            //Description = "Has Extra talk tomorrow",
                            //Status = "Is Speaked",
                            //DisplayName = _members.FirstOrDefault(p => p.MemberId == speak.MemberId)?.UserId,
                            EventType = GameEventType.Speak
                        });
                    }
                }

                if (silenced != null)
                {
                    await _unitOfWork.GameMember.SilenceMember(silenced.MemberId);
                    results.Add(new GameEventResult()
                    {
                        MemberId = silenced.MemberId,
                        //Description = "Can't Talk tomorrow",
                        //Status = "Is Silenced",
                        //DisplayName = _members.FirstOrDefault(p => p.MemberId == silenced.MemberId)?.UserId,
                        EventType = GameEventType.Silenced
                    });
                }

                if (sniped != null)
                {
                    var targets = await _unitOfWork.GameMember.GetPlayerStatusFast(sniped.MemberId);
                    if (targets.Any())
                    {
                        var target = targets.FirstOrDefault();
                        //if sniper shoots a mafia he will be killed himself
                        if (target.GameRole == GameRole.GodFather || target.GameRole == GameRole.Mafia ||
                            target.GameRole == GameRole.Terrorist)
                        {
                            var snipers =
                                await _unitOfWork.GameMember.GetPlayerByRoleFast(gameId, (short) GameRole.Sniper);
                            if (snipers.Any())
                            {
                                var sniper = snipers.FirstOrDefault();
                                await _unitOfWork.GameMember.KillMember(sniper.MemberId);
                                results.Add(new GameEventResult()
                                {
                                    MemberId = sniper.MemberId,
                                    //Description = "Can no longer play or vote",
                                    //Status = "Is Killed",
                                    //DisplayName = _members.FirstOrDefault(p => p.MemberId == sniper.MemberId)?.UserId,
                                    EventType = GameEventType.Killed
                                });
                            }
                        }
                        else
                        {
                            await _unitOfWork.GameMember.KillMember(sniped.MemberId);
                            results.Add(new GameEventResult()
                            {
                                MemberId = sniped.MemberId,
                                //Description = "Can no longer play or vote",
                                //Status = "Is Killed",
                                //DisplayName = _members.FirstOrDefault(p => p.MemberId == sniped.MemberId)?.UserId,
                                EventType = GameEventType.Killed
                            });
                        }
                    }
                }

                _cache.SetCache(results, $"NightResults-{gameId}");
                try
                {
                    await _gameHub.Clients.Group(gameId).SendAsync("NightResult");
                    //remove player status cache
                    CacheFactory.GetCache().Remove($"GamePlayers-{gameId}");
                    //check if game finished
                    finish = await CheckFinish(gameId);
                }
                catch (ItemNotFoundException)
                {
                }
            }
            else
                await _gameHub.Clients.Group(gameId).SendAsync("NightResult");

            if (!finish)
                BackgroundJob.Schedule(() => ValidateGameEvents(gameId), TimeSpan.FromSeconds(20));
        }

        public async Task<bool> CheckFinish(string gameId)
        {
            var members = await _unitOfWork.GameMember.GetPlayerByGame(gameId);
            var mafia = members.Where(m =>
                m.Role.IsMafia() && m.Status != PlayerStatus.Killed && m.Status != PlayerStatus.Kicked);
            var citizen = members.Where(m =>
                m.Role.IsCitizen() && m.Status != PlayerStatus.Killed && m.Status != PlayerStatus.Kicked);
            if (mafia.Count() >= citizen.Count())
            {
                //mafia wins
                await _unitOfWork.Game.MafiaWin(gameId);
                await _gameHub.Clients.Group(gameId).SendAsync("GameFinish", "mafia");
                return true;
            }

            if (mafia.Any()) 
                return false;
            //citizen wins
            await _unitOfWork.Game.CitizenWin(gameId);
            await _gameHub.Clients.Group(gameId).SendAsync("GameFinish", "citizen");
            return true;

        }

        public async Task Inquiry(string gameId, string memberId, bool isToggled)
        {
            var targets = await _unitOfWork.GameMember.GetPlayerStatusFast(memberId);
            if (!targets.Any())
                return;
            var target = targets.FirstOrDefault();
            if (target.GameRole is GameRole.Mafia or GameRole.Terrorist)
                await _gameHub.Clients.Group(gameId).SendAsync("Inquiry", !isToggled);
            else
                await _gameHub.Clients.Group(gameId).SendAsync("Inquiry", isToggled);
        }

        public async Task ValidateGameEvents(string gameId)
        {
            await _unitOfWork.GameEvent.Validate(gameId);
            BackgroundJob.Enqueue(() => SetTurn(gameId, 0));
        }
    }
}
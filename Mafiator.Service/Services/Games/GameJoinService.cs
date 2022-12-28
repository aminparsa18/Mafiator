using Hangfire;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Extensions;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Mafiator.Service.Contracts.Games;
using Mafiator.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Games;

public class GameJoinService : IGameJoinService
{
    private readonly IGameLogicService _gameService;
    private readonly IHubContext<GameHub> _gameHub;
    private readonly ILiveEventManager _liveEventManager;
    private readonly IUnitOfWork _unitOfWork;

    public GameJoinService(IGameLogicService gameService, ILiveEventManager liveEventManager, IHubContext<GameHub> gameHub, IUnitOfWork unitOfWork)
    {
        _gameService = gameService;
        _gameHub = gameHub;
        _liveEventManager = liveEventManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult> Join(string userId, string gameId)
    {
        var members = await _unitOfWork.GameMember.GetUsersByGame(gameId);
        //if (!members.Any())
        //    return Ok(new ApiResult()
        //    {
        //        IsSuccess = false,
        //        Errors = new[] {"No such game!!!"},
        //        StatusCode = ApiResultStatusCode.NotFound
        //    });
        if (!members.Any(m => string.IsNullOrEmpty(m.UserId)))
        {
            return new ApiResult
            {
                IsSuccess = false,
                Errors = new[] { "Sorry, no more capacity for this game" },
                StatusCode = ApiResultStatusCode.BadRequest
            };
        }

        if (members.Any(m => m.UserId == userId))
            return new ApiResult
            {
                IsSuccess = false,
                Errors = new[] { "You are already part of this game!!!" },
                StatusCode = ApiResultStatusCode.Conflict
            };
        var selected = members.Where(m => string.IsNullOrEmpty(m.UserId)).SelectRandom();
        await _unitOfWork.GameMember.Join(userId, selected.MemberId);
        await _gameHub.Clients.Group(gameId).SendAsync("Join", userId);
        if (members.Count(m => string.IsNullOrEmpty(m.UserId)) != 1)
            return new ApiResult
            {
                IsSuccess = true
            };

        await _gameHub.Clients.Group(gameId).SendAsync("StartLiveEvent");
        var live = await _liveEventManager.CreateLiveEvent(gameId);
        await _gameHub.Clients.Group(gameId).SendAsync("StartGame", live.Item1, live.Item2);
        BackgroundJob.Schedule(() => _gameService.SetTurn(gameId, 0), TimeSpan.FromSeconds(30));
        await _unitOfWork.Game.StartGame(gameId);
        return new ApiResult
        {
            IsSuccess = true
        };
    }
}
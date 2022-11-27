using Ardalis.ApiEndpoints;
using Hangfire;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Extensions;
using Mafiator.IocConfig.Hubs;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
[Produces("application/x-msgpack")]
public class Join : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult>
{
    private readonly ILiveEventManager _liveEventManager;
    private readonly IGameService _gameService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<GameHub> _gameHub;

    public Join(ILiveEventManager liveEventManager, IGameService gameService, IUnitOfWork unitOfWork,
           IHubContext<GameHub> gameHub)
    {
        _gameService = gameService;
        _liveEventManager = liveEventManager;
        _unitOfWork = unitOfWork;
        _gameHub = gameHub;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/games/join/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Join), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
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
            return Ok(new ApiResult()
            {
                IsSuccess = false,
                Errors = new[] { "Sorry,no more capacity for this game" },
                StatusCode = ApiResultStatusCode.BadRequest
            });
        }

        var userId = User.FindFirstValue(ClaimTypes.Name);
        if (members.Any(m => m.UserId == userId))
            return Ok(new ApiResult()
            {
                IsSuccess = false,
                Errors = new[]
                    {"You are already part of this game!!!"},
                StatusCode = ApiResultStatusCode.Conflict
            });
        var selected = members.Where(m => string.IsNullOrEmpty(m.UserId)).SelectRandom();
        await _unitOfWork.GameMember.Join(userId, selected.MemberId);
        await _gameHub.Clients.Group(gameId).SendAsync("Join", userId);
        if (members.Count(m => string.IsNullOrEmpty(m.UserId)) != 1)
            return Ok(new ApiResult()
            {
                IsSuccess = true
            });

        await _gameHub.Clients.Group(gameId).SendAsync("StartLiveEvent");
        var live = await _liveEventManager.CreateLiveEvent(gameId);
        await _gameHub.Clients.Group(gameId).SendAsync("StartGame", live.Item1, live.Item2);
        BackgroundJob.Schedule(() => _gameService.SetTurn(gameId, 0), TimeSpan.FromSeconds(30));
        await _unitOfWork.Game.StartGame(gameId);

        return Ok(new ApiResult()
        {
            IsSuccess = true
        });
    }
}
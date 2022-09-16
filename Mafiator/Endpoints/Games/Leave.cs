using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.IocConfig.Hubs;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NSwag.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
[Produces("application/x-msgpack")]
public class Leave : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<GameHub> _gameHub;

    public Leave(IUnitOfWork unitOfWork, IHubContext<GameHub> gameHub)
    {
        _unitOfWork = unitOfWork;
        _gameHub = gameHub;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/games/leave/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Games.Leave", "", "Leaves a game.")]
    [OpenApiTag("Games Endpoints")]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var member = await _unitOfWork.GameMember.GetUser(userId, gameId);
        if (!member.Any())
            return Ok(new ApiResult<string>()
            {
                IsSuccess = false,
                Errors = new[] { "You are not member of this game" },
                StatusCode = ApiResultStatusCode.NotFound
            });
        await _unitOfWork.GameMember.Leave(userId);
        await _gameHub.Clients.Group(gameId).SendAsync("Join", userId.ToString());
        return Ok(new ApiResult<string>()
        {
            IsSuccess = true,
        });
    }
}
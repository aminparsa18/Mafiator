using Mafiator.Service.Contracts.Games;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
[Produces("application/x-msgpack")]
public class Leave : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IGameLeaveService _gameLeaveService;

    public Leave(IGameLeaveService gameLeaveService)
    {
        _gameLeaveService = gameLeaveService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/games/leave/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Leave), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _gameLeaveService.Leave(userId, gameId));
    }
}
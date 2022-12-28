using Mafiator.Service.Contracts.Games;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
[Produces("application/x-msgpack")]
public class Join : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult>
{
    private readonly IGameJoinService _gameJoinService;

    public Join(IGameJoinService gameJoinService)
    {
        _gameJoinService = gameJoinService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/games/join/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Join), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _gameJoinService.Join(userId, gameId));
    }
}
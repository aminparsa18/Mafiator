using Mafiator.Service.Contracts.Games;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class IsJoined : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IGameService _gameService;

    public IsJoined(IGameService gameService)
    {
        _gameService = gameService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/isJoined/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(IsJoined), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _gameService.IsJoined(userId, gameId));
    }
}
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts.GameEvents;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
[Produces("application/x-msgpack")]
public class Cure : EndpointBaseAsync
    .WithRequest<GameEventRequest>
    .WithActionResult<ApiResult>
{
    private readonly IGameEventCureService _gameEventCureService;

    public Cure(IGameEventCureService gameEventCureService)
    {
        _gameEventCureService = gameEventCureService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/gameevents/cure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Cure), Tags = new[] { "Game Event Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(GameEventRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _gameEventCureService.Cure(userId, request));
    }
}
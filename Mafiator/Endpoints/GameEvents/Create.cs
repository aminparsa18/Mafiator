using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts.GameEvents;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<GameEventRequest>
    .WithActionResult<ApiResult>
{
    private readonly IGameEventCreateService _gameEventCreateService;

    public Create(IGameEventCreateService gameEventCreateService)
    {
        _gameEventCreateService = gameEventCreateService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/gameevents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Game Events Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(GameEventRequest request, CancellationToken cancellationToken = default) =>
        Ok(await _gameEventCreateService.Create(request));
}
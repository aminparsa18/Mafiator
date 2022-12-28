using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<GameCreateRequest>
    .WithActionResult<ApiResult<GameCreateResult>>
{
    private readonly IGameCreateService _gameCreateService;

    public Create(IGameCreateService gameCreateService)
    {
        _gameCreateService = gameCreateService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/games")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<GameCreateResult>>> HandleAsync(GameCreateRequest request, CancellationToken cancellationToken = default) =>
        Ok(await _gameCreateService.Create(request));
}
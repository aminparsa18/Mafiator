using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetAppointed : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<AppointedGameResult>>
{
    private readonly IGameService _gameService;

    public GetAppointed(IGameService gameService)
    {
        _gameService = gameService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/appointed/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetAppointed), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<AppointedGameResult>>> HandleAsync(string roomId, CancellationToken cancellationToken = default) =>
        Ok(await _gameService.GetAppointed(Guid.Parse(roomId)));
}
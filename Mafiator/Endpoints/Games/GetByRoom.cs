using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<RoomGameResult>>>
{
    private readonly IGameService _gameService;

    public GetByRoom(IGameService gameService)
    {
        _gameService = gameService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByRoom), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<RoomGameResult>>>> HandleAsync(string roomId, CancellationToken cancellationToken = default) =>
        Ok(await _gameService.GetByRoom(roomId));
}
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetAvailables : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<AvailableGameResult>>>
{
    private readonly IGameService _gameService;

    public GetAvailables(IGameService gameService)
    {
        _gameService = gameService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/availables")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetAvailables), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<AvailableGameResult>>>> HandleAsync(CancellationToken cancellationToken = default) => 
        Ok(await _gameService.GetAvailable());
}
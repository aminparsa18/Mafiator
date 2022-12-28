using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetAppointedDetails : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<AppointedGameResult>>
{
    private readonly IGameService _gameService;

    public GetAppointedDetails(IGameService gameService)
    {
        _gameService = gameService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/appointed-details/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetAppointedDetails), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<AppointedGameResult>>> HandleAsync(string gameId, CancellationToken cancellationToken = default) => 
        Ok(await _gameService.GetAppointedDetails(Guid.Parse(gameId)));
}
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameMembers;

[Authorize]
public class GetWaitingPlayers : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<WaitingPlayerResult>>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetWaitingPlayers(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/waiting/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetWaitingPlayers), Tags = new[] { "Game members Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<WaitingPlayerResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default) =>
        Ok(await _gameMemberService.GetWaitingPlayersByGame(gameId));
}
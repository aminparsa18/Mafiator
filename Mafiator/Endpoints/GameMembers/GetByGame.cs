using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameMembers;

[Authorize]
public class GetByGame : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<GameMemberResult>>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetByGame(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByGame), Tags = new[] { "Game members Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<GameMemberResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default) =>
        Ok(await _gameMemberService.GetByGame(gameId));
}
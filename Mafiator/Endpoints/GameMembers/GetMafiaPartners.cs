using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.GameMembers;

[Authorize]
public class GetMafiaPartners : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<PlayerRoleResult>>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetMafiaPartners(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/mafia-partners/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetMafiaPartners), Tags = new[] { "Game members Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<PlayerRoleResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _gameMemberService.GetMafiaPartners(userId, gameId));
    }
}
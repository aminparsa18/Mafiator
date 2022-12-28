using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.GameMembers;

[Authorize]
public class GetPlayerRole : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<PlayerRoleResult>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetPlayerRole(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/role/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetPlayerRole), Tags = new[] { "Game members Endpoints" })]
    public override async Task<ActionResult<ApiResult<PlayerRoleResult>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _gameMemberService.GetPlayerRole(userId, gameId));
    }
}
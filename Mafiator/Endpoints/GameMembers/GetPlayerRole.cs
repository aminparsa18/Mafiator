using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.GameMembers;

public class GetPlayerRole : EndpointWithoutRequest<ApiResult<PlayerRoleResult>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetPlayerRole(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    public override void Configure()
    {
        Get("api/v1/gamemembers/role/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.GameMembers));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(await _gameMemberService.GetPlayerRole(userId, gameId), cancellation: ct);
    }
}
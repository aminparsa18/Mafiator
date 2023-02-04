using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.GameMembers;

public class GetMafiaPartners : EndpointWithoutRequest<ApiResult<IEnumerable<PlayerRoleResult>>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetMafiaPartners(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    public override void Configure()
    {
        Get("api/v1/gamemembers/mafia-partners/{gameId}");
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
        await SendMemoryPackAsync(await _gameMemberService.GetMafiaPartners(userId, gameId), cancellation: ct);
    }
}
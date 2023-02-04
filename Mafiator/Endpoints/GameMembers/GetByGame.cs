using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameMembers;

[Authorize]
public class GetByGame : EndpointWithoutRequest<ApiResult<IEnumerable<GameMemberResult>>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetByGame(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    public override void Configure()
    {
        Get("api/v1/gamemembers/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.GameMembers));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(await _gameMemberService.GetByGame(gameId), cancellation: ct);
    }
}
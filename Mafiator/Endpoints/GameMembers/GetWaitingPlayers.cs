using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameMembers;

public class GetWaitingPlayers : EndpointWithoutRequest<ApiResult<IEnumerable<WaitingPlayerResult>>>
{
    private readonly IGameMemberService _gameMemberService;

    public GetWaitingPlayers(IGameMemberService gameMemberService)
    {
        _gameMemberService = gameMemberService;
    }

    public override void Configure()
    {
        Get("api/v1/gamemembers/waiting/{gameId}");
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
        await SendMemoryPackAsync(await _gameMemberService.GetWaitingPlayersByGame(gameId), cancellation: ct);
    }
}
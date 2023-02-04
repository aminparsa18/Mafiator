using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Service.Contracts.Votes;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Votes;

public class GetByGame : EndpointWithoutRequest<ApiResult<IEnumerable<VoteDetailsResult>>>
{
    private readonly IVoteService _voteService;

    public GetByGame(IVoteService voteService)
    {
        _voteService = voteService;
    }

    public override void Configure()
    {
        Get("api/v1/votes/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Votes));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(await _voteService.GetByGame(gameId), cancellation: ct);
    }
}
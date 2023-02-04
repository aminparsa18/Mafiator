using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Service.Contracts.Votes;

namespace Mafiator.Api.Endpoints.Votes;

public class Create : Endpoint<VoteCreateRequest,ApiResult>
{
    private readonly IVoteCreateService _voteCreateService;

    public Create(IVoteCreateService voteCreateService)
    {
        _voteCreateService = voteCreateService;
    }

    public override void Configure()
    {
        Post("api/v1/votes");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Votes));
    }

    public override async Task HandleAsync(VoteCreateRequest request, CancellationToken ct) => 
        await SendMemoryPackAsync(await _voteCreateService.Create(request), cancellation: ct);
}
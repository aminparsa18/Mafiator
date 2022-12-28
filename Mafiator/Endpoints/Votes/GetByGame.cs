using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Service.Contracts.Votes;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Votes;

[Authorize]
public class GetByGame : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<VoteDetailsResult>>>
{
    private readonly IVoteService _voteService;

    public GetByGame(IVoteService voteService)
    {
        _voteService = voteService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/votes/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByGame), Tags = new[] { "Votes Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<VoteDetailsResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default) =>
        Ok(await _voteService.GetByGame(gameId));
}
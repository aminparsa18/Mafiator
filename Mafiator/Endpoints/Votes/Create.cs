using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Service.Contracts.Votes;

namespace Mafiator.Api.Endpoints.Votes;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<VoteCreateRequest>
    .WithActionResult<ApiResult>
{
    private readonly IVoteCreateService _voteCreateService;

    public Create(IVoteCreateService voteCreateService)
    {
        _voteCreateService = voteCreateService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/votes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Votes Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(VoteCreateRequest request, CancellationToken cancellationToken = default) =>
        Ok(await _voteCreateService.Create(request));
}
using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Votes;

[Authorize]
public class GetByGame : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<VoteDetailsResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByGame(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/votes/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByGame), Tags = new[] { "Votes Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<VoteDetailsResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResult<IEnumerable<VoteDetailsResult>>()
        {
            Data = await _unitOfWork.Vote.GetVoteStatus(gameId),
            IsSuccess = true
        });
    }
}
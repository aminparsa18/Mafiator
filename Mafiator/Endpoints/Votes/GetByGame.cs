using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Votes;

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
    [HttpGet("api/v{version:apiVersion}/votes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Votes.GetByGame", "Retrieves all votes statuses of game.")]
    [OpenApiTag("Votes Endpoints")]
    public override async Task<ActionResult<ApiResult<IEnumerable<VoteDetailsResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResult<IEnumerable<VoteDetailsResult>>()
        {
            Data = await _unitOfWork.Vote.GetVoteStatus(gameId),
            IsSuccess = true
        });
    }
}
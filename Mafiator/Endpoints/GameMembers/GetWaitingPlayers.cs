using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameMembers;

public class GetWaitingPlayers : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<WaitingPlayerResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWaitingPlayers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/waiting/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("GameMembers.GetWaitingPlayer", "", "Retrieves all waiting players for the game to be started.")]
    [OpenApiTag("GameMembers Endpoints")]
    public override async Task<ActionResult<ApiResult<IEnumerable<WaitingPlayerResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var members = await _unitOfWork.GameMember.GetWaitingPlayersByGame(gameId);
        foreach (var gameMemberDto in members)
        {
            gameMemberDto.Image = Constants.BlobStorageEndpoint + gameMemberDto.Image;
        }
        return Ok(new ApiResult<IEnumerable<WaitingPlayerResult>>()
        {
            IsSuccess = true,
            Data = members
        });
    }
}
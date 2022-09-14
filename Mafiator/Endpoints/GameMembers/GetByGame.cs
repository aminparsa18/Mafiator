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

[Produces("application/x-msgpack")]
[Consumes("application/x-msgpack")]
public class GetByGame : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<GameMemberResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByGame(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("GameMembers.GetByGame", "", "Retrieves all members in a game.")]
    [OpenApiTag("GameMembers Endpoints")]
    public override async Task<ActionResult<ApiResult<IEnumerable<GameMemberResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var members = await _unitOfWork.GameMember.GetByGameFast(gameId);
        foreach (var gameMemberDto in members)
        {
            gameMemberDto.Image = Constants.BlobStorageEndpoint + gameMemberDto.Image;
        }
        return Ok(new ApiResult<IEnumerable<GameMemberResult>>()
        {
            IsSuccess = true,
            Data = members
        });
    }
}
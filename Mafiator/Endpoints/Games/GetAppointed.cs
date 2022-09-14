using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Produces("application/x-msgpack")]
[Consumes("application/x-msgpack")]
public class GetAppointed : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<AppointedGameResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAppointed(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/appointed/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Games.GetAppointed", "", "Retrieves appointed game in a room.")]
    [OpenApiTag("Games Endpoints")]
    public override async Task<ActionResult<ApiResult<AppointedGameResult>>> HandleAsync
        (string roomId, CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.Game.GetWaitingGameByRoom(Guid.Parse(roomId));
        data?.Members.ForEach(m => m.Image = Constants.BlobStorageEndpoint + m.Image);
        return Ok(new ApiResult<AppointedGameResult>
        {
            IsSuccess = true,
            Data = data
        });
    }
}
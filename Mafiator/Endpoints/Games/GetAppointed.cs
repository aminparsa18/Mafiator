using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
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
    [SwaggerOperation(OperationId = nameof(GetAppointed), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<AppointedGameResult>>> HandleAsync
        (string roomId, CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.Game.GetWaitingGameByRoom(Guid.Parse(roomId));
        data?.Members.ForEach(m => m.Image = Data.Constants.BlobStorageEndpoint + m.Image);
        return Ok(new ApiResult<AppointedGameResult>
        {
            IsSuccess = true,
            Data = data
        });
    }
}
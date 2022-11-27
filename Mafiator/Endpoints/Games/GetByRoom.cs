using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<RoomGameResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByRoom(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByRoom), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<RoomGameResult>>>> HandleAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.Game.GetByRoom(roomId);
        return Ok(new ApiResult<IEnumerable<RoomGameResult>>
        {
            IsSuccess = true,
            Data = data
        });
    }
}
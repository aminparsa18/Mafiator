using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
public class IsJoined : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public IsJoined(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/rooms/isJoined/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(IsJoined), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var member = await _unitOfWork.Room.IsJoinedFast(userId, roomId);
        return Ok(new ApiResult<string>()
        {
            IsSuccess = true,
            Data = member
        });
    }
}
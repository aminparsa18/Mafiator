using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
public class IsJoined : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IRoomService _roomService;

    public IsJoined(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/rooms/isJoined/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(IsJoined), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomService.IsJoined(userId, roomId));
    }
}
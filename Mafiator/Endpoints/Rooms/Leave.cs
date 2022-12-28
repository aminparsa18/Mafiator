using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class Leave : EndpointBaseAsync
    .WithRequest<LeaveRoomRequest>
    .WithActionResult<ApiResult>
{
    private readonly IRoomLeaveService _roomLeaveService;

    public Leave(IRoomLeaveService roomLeaveService)
    {
        _roomLeaveService = roomLeaveService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Leave), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomLeaveService.Leave(userId, request));
    }
}
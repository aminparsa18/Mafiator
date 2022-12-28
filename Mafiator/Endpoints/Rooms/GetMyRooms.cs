using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
public class GetMyRooms : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<RoomDetailsResult>>
{
    private readonly IRoomService _roomService;

    public GetMyRooms(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetMyRooms), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult<RoomDetailsResult>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomService.GetMyRooms(userId));
    }
}
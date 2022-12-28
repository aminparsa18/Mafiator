using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
public class Get : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<RoomDetailsResult>>
{
    private readonly IRoomService _roomService;

    public Get(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/rooms/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult<RoomDetailsResult>>> HandleAsync(string roomId, CancellationToken cancellationToken = default) =>
        Ok(await _roomService.GetDetails(roomId));
}
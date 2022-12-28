using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class Update : EndpointBaseAsync
    .WithRequest<UpdateRoomNameRequest>
    .WithActionResult<ApiResult>
{
    private readonly IRoomUpdateService _roomUpdateService;

    public Update(IRoomUpdateService roomUpdateService)
    {
        _roomUpdateService = roomUpdateService;
    }

    [ApiVersion("1.0")]
    [HttpPut("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Update), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(UpdateRoomNameRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomUpdateService.Update(userId, request));
    }
}
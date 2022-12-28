using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<RoomCreateRequest>
    .WithActionResult<ApiResult<RoomCreateResult>>
{
    private readonly IRoomCreateService _roomCreateService;

    public Create(IRoomCreateService roomCreateService)
    {
        _roomCreateService = roomCreateService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult<RoomCreateResult>>> HandleAsync(RoomCreateRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomCreateService.Create(userId, request));
    }
}
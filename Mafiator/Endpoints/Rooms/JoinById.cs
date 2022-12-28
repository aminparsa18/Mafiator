using Mafiator.Service.Contracts.Rooms;
using System;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class JoinById : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ApiResult>
{
    private readonly IRoomJoinService _roomJoinService;

    public JoinById(IRoomJoinService roomJoinService)
    {
        _roomJoinService = roomJoinService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/join/id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(JoinById), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomJoinService.JoinById(userId, id));
    }
}
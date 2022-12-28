using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class JoinByCode : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IRoomJoinService _roomJoinService;

    public JoinByCode(IRoomJoinService roomJoinService)
    {
        _roomJoinService = roomJoinService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/join/code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(JoinByCode), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string code, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _roomJoinService.JoinByCode(userId, code));
    }
}
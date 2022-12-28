using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Service.Contracts.RoomMembers;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.RoomMembers;

[Authorize]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<RoomMemberResult>>>
{
    private readonly IRoomMemberService _roomMemberService;

    public GetByRoom(IRoomMemberService roomMemberService)
    {
        _roomMemberService = roomMemberService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/roommembers/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByRoom), Tags = new[] { "Room member Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<RoomMemberResult>>>> HandleAsync(string roomId, CancellationToken cancellationToken = default)
    {
        return Ok(await _roomMemberService.GetByRoom(roomId));
    }
}
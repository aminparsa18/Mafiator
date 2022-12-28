using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Service.Contracts.RoomMembers;

namespace Mafiator.Api.Endpoints.RoomMembers;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<NewMembersRequest>
    .WithActionResult<ApiResult>
{
    private readonly IRoomMemberCreateService _roomMemberCreateService;

    public Create(IRoomMemberCreateService roomMemberCreateService)
    {
        _roomMemberCreateService = roomMemberCreateService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/roommembers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Room member Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(NewMembersRequest request, CancellationToken cancellationToken = default) =>
        Ok(await _roomMemberCreateService.Create(request));
}
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Service.Contracts.RoomMembers;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.RoomMembers;

public class GetByRoom : EndpointWithoutRequest<ApiResult<IEnumerable<RoomMemberResult>>>
{
    private readonly IRoomMemberService _roomMemberService;

    public GetByRoom(IRoomMemberService roomMemberService)
    {
        _roomMemberService = roomMemberService;
    }

    public override void Configure()
    {
        Get("api/v1/games/roommembers/{roomId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.RoomMembers));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string roomId = Route<string>("roomId");
        await SendMemoryPackAsync(await _roomMemberService.GetByRoom(roomId), cancellation: ct);
    }
}
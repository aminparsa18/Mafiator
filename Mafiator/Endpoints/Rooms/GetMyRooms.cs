using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class GetMyRooms : EndpointWithoutRequest<ApiResult<IEnumerable<RoomDetailsResult>>>
{
    private readonly IRoomService _roomService;

    public GetMyRooms(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public override void Configure()
    {
        Get("api/v1/rooms");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Rooms));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _roomService.GetMyRooms(userId), cancellation: ct);
    }
}
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class Leave : Endpoint<LeaveRoomRequest,ApiResult>
{
    private readonly IRoomLeaveService _roomLeaveService;

    public Leave(IRoomLeaveService roomLeaveService)
    {
        _roomLeaveService = roomLeaveService;
    }

    public override void Configure()
    {
        Post("api/v1/rooms/leave");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Rooms));
    }

    public override async Task HandleAsync(LeaveRoomRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _roomLeaveService.Leave(userId, request));
    }
}
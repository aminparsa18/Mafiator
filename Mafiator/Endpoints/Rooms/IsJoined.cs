using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class IsJoined : EndpointWithoutRequest<ApiResult<string>>
{
    private readonly IRoomService _roomService;

    public IsJoined(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public override void Configure()
    {
        Get("api/v1/rooms/isJoined/{roomId}");
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
        string roomId = Route<string>("roomId");
        await SendMemoryPackAsync(await _roomService.IsJoined(userId, roomId), cancellation: ct);
    }
}
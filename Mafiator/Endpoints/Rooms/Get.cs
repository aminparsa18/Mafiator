using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;

namespace Mafiator.Api.Endpoints.Rooms;

public class Get : EndpointWithoutRequest<ApiResult<RoomDetailsResult>>
{
    private readonly IRoomService _roomService;

    public Get(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public override void Configure()
    {
        Get("api/v1/rooms/{roomId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Rooms));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string roomId = Route<string>("roomId");
        await SendMemoryPackAsync(await _roomService.GetDetails(roomId), cancellation: ct);
    }
}
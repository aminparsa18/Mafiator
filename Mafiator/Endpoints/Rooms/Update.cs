using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class Update : Endpoint<UpdateRoomNameRequest,ApiResult>
{
    private readonly IRoomUpdateService _roomUpdateService;

    public Update(IRoomUpdateService roomUpdateService)
    {
        _roomUpdateService = roomUpdateService;
    }

    public override void Configure()
    {
        Put("api/v1/rooms");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Rooms));
    }

    public override async Task HandleAsync(UpdateRoomNameRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _roomUpdateService.Update(userId, request), cancellation: ct);
    }
}
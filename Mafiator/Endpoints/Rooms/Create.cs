using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class Create : Endpoint<RoomCreateRequest, ApiResult<RoomCreateResult>>
{
    private readonly IRoomCreateService _roomCreateService;

    public Create(IRoomCreateService roomCreateService)
    {
        _roomCreateService = roomCreateService;
    }

    public override void Configure()
    {
        Post("api/v1/rooms");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d =>d.Produces(200).WithTags(EndpointsTags.Rooms));
    }

    public override async Task HandleAsync(RoomCreateRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _roomCreateService.Create(userId, request));
    }
}
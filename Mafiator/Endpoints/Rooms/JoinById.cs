using Mafiator.Service.Contracts.Rooms;
using System;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class JoinById : EndpointWithoutRequest<ApiResult>
{
    private readonly IRoomJoinService _roomJoinService;

    public JoinById(IRoomJoinService roomJoinService)
    {
        _roomJoinService = roomJoinService;
    }

    public override void Configure()
    {
        Get("api/v1/rooms/join/id/{id}");
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
        Guid id = Route<Guid>("id");
        await SendMemoryPackAsync(await _roomJoinService.JoinById(userId, id), cancellation: ct);
    }
}
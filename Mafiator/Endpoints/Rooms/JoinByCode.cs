using Mafiator.Service.Contracts.Rooms;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Rooms;

public class JoinByCode : EndpointWithoutRequest<ApiResult<string>>
{
    private readonly IRoomJoinService _roomJoinService;

    public JoinByCode(IRoomJoinService roomJoinService)
    {
        _roomJoinService = roomJoinService;
    }

    public override void Configure()
    {
        Post("api/v1/rooms/join/code/{code}");
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
        string code = Route<string>("code");
        await SendMemoryPackAsync(await _roomJoinService.JoinByCode(userId, code), cancellation: ct);
    }
}
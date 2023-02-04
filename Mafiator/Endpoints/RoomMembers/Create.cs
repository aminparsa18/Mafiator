using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Service.Contracts.RoomMembers;

namespace Mafiator.Api.Endpoints.RoomMembers;

public class Create : Endpoint<NewMembersRequest,ApiResult>
{
    private readonly IRoomMemberCreateService _roomMemberCreateService;

    public Create(IRoomMemberCreateService roomMemberCreateService)
    {
        _roomMemberCreateService = roomMemberCreateService;
    }

    public override void Configure()
    {
        Post("api/v1/roommembers");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.RoomMembers));
    }

    public override async Task HandleAsync(NewMembersRequest request, CancellationToken ct) =>
        await SendMemoryPackAsync(await _roomMemberCreateService.Create(request), cancellation: ct);
}
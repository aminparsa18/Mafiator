using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Service.Contracts.Avatars;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Avatars;

public class GetFast : EndpointWithoutRequest<ApiResult<IEnumerable<AvatarResult>>>
{
    private readonly IAvatarService _avatarService;

    public GetFast(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }

    public override void Configure()
    {
        Get(ApiUrls.Avatars);
        Summary(s =>
        {
            s.Summary = "Get Avatars";
            s.Description = "Retrieves all avatars data.";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Avatars));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var avatars = await _avatarService.GetAll();
        await SendMemoryPackAsync(new ApiResult<IEnumerable<AvatarResult>>
        {
            Data = avatars,
            IsSuccess = true
        }, cancellation: ct);
    }
}
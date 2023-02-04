using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Users;

public class Update : Endpoint<UpdateProfileRequest,ApiResult>
{
    private readonly IUserUpdateService _userUpdateService;

    public Update(IUserUpdateService userUpdateService)
    {
        _userUpdateService = userUpdateService;
    }

    public override void Configure()
    {
        Post("api/v1/users/update");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Users));
    }

    public override async Task HandleAsync(UpdateProfileRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _userUpdateService.Update(userId, request), cancellation: ct);
    }
}
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Users;

public class Get : EndpointWithoutRequest<ApiResult<UserDetailsResult>>
{
    private readonly IUserService _userService;

    public Get(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("api/v1/users");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Users));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _userService.GetDetails(userId), cancellation: ct);
    }
}
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;

namespace Mafiator.Api.Endpoints.Users;

public class Login : Endpoint<UserLoginRequest,AuthResult>
{
    private readonly IUserLoginService _userLoginService;

    public Login(IUserLoginService userLoginService)
    {
        _userLoginService = userLoginService;
    }

    public override void Configure()
    {
        Post("api/v1/users/login");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Users));
    }

    public override async Task HandleAsync(UserLoginRequest request, CancellationToken ct) =>
        await SendMemoryPackAsync(await _userLoginService.Login(request), cancellation: ct);
}
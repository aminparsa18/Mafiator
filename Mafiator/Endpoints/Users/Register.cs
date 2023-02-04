using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;

namespace Mafiator.Api.Endpoints.Users;

public class Register : Endpoint<RegisterUserRequest, ApiResult>
{
    private readonly IUserRegisterService _userRegisterService;

    public Register(IUserRegisterService userRegisterService)
    {
        _userRegisterService = userRegisterService;
    }

    public override void Configure()
    {
        Post("api/v1/users/register");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Users));
    }

    public override async Task HandleAsync(RegisterUserRequest request, CancellationToken ct) =>
        await SendMemoryPackAsync(await _userRegisterService.Register(request), cancellation: ct);
}
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;

namespace Mafiator.Api.Endpoints.Users;

public class Confirm : Endpoint<ConfirmPhoneRequest,AuthResult>
{
    private readonly IUserConfirmService _userConfirmService;

    public Confirm(IUserConfirmService userConfirmService)
    {
        _userConfirmService = userConfirmService;
    }

    public override void Configure()
    {
        Post("api/v1/users/confirm");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Users));
    }

    public override async Task HandleAsync(ConfirmPhoneRequest request, CancellationToken ct) =>
        await SendMemoryPackAsync(await _userConfirmService.Confirm(request), cancellation: ct);
}
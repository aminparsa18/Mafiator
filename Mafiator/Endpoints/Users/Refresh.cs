using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Service.Contracts.RefreshTokens;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
public class Refresh : Endpoint<RefreshTokenRequest,AuthResult>
{
    private readonly IRefreshTokenService _refreshTokenService;

    public Refresh(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    public override void Configure()
    {
        Post("api/v1/users/refresh");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Users));
    }

    public override async Task HandleAsync(RefreshTokenRequest request, CancellationToken ct) => 
        await SendMemoryPackAsync(await _refreshTokenService.Refresh(request), cancellation: ct);
}
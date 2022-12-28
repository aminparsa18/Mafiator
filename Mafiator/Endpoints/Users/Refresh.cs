using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Service.Contracts.RefreshTokens;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
public class Refresh : EndpointBaseAsync
    .WithRequest<RefreshTokenRequest>
    .WithActionResult<AuthResult>
{
    private readonly IRefreshTokenService _refreshTokenService;

    public Refresh(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Refresh), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<AuthResult>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        return Ok(await _refreshTokenService.Refresh(request));
    }
}
using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
[Consumes("application/x-msgpack")]
public class Refresh : EndpointBaseAsync
    .WithRequest<RefreshTokenRequest>
    .WithActionResult<AuthResult>
{
    private readonly IIdentityService _identityService;

    public Refresh(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Refresh", "", "Refreshes user expired jwt token.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<AuthResult>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.RefreshToken(request);
        return Ok(result);
    }
}
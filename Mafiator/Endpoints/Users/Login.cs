using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
public class Login : EndpointBaseAsync
    .WithRequest<UserLoginRequest>
    .WithActionResult<AuthResult>
{
    private readonly IIdentityService _identityService;

    public Login(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Login", "", "login user.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<AuthResult>> HandleAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Ok(new AuthResult()
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(s => s.ErrorMessage))
            });
        var result = await _identityService.Login(request);
        return Ok(result);
    }
}
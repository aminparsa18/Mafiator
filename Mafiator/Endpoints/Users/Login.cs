using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
public class Login : EndpointBaseAsync
    .WithRequest<UserLoginRequest>
    .WithActionResult<AuthResult>
{
    private readonly IUserLoginService _userLoginService;

    public Login(IUserLoginService userLoginService)
    {
        _userLoginService = userLoginService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Login), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<AuthResult>> HandleAsync(UserLoginRequest request, CancellationToken cancellationToken = default) => 
        Ok(await _userLoginService.Login(request));
}
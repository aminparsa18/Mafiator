using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;

namespace Mafiator.Api.Endpoints.Users;

//[Produces("application/x-msgpack")]
public class Register : EndpointBaseAsync
    .WithRequest<RegisterUserRequest>
    .WithActionResult<ApiResult>
{
    private readonly IUserRegisterService _userRegisterService;

    public Register(IUserRegisterService userRegisterService)
    {
        _userRegisterService = userRegisterService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Register), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken = default) =>
        Ok(await _userRegisterService.Register(request));
}
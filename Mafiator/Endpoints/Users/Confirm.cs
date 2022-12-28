using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
public class Confirm : EndpointBaseAsync
    .WithRequest<ConfirmPhoneRequest>
    .WithActionResult<AuthResult>
{
    private readonly IUserConfirmService _userConfirmService;

    public Confirm(IUserConfirmService userConfirmService)
    {
        _userConfirmService = userConfirmService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Confirm), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<AuthResult>> HandleAsync(ConfirmPhoneRequest request, CancellationToken cancellationToken = default) =>
        Ok(await _userConfirmService.Confirm(request));
}
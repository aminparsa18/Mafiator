using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
[Produces("application/x-msgpack")]
public class Update : EndpointBaseAsync
    .WithRequest<UpdateProfileRequest>
    .WithActionResult<ApiResult>
{
    private readonly IUserUpdateService _userUpdateService;

    public Update(IUserUpdateService userUpdateService)
    {
        _userUpdateService = userUpdateService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Update), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _userUpdateService.Update(userId, request));
    }
}
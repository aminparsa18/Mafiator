using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
public class Status : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<UserStatusResult>>
{
    private readonly IUserService _userService;

    public Status(IUserService userService)
    {
        _userService = userService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/users/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Status), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult<UserStatusResult>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _userService.GetStatus(userId));
    }
}
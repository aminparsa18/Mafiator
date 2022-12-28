using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Users;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<UserDetailsResult>>
{
    private readonly IUserService _userService;

    public Get(IUserService userService)
    {
        _userService = userService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult<UserDetailsResult>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _userService.GetDetails(userId));
    }
}
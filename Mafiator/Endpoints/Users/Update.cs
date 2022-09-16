using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
[Produces("application/x-msgpack")]
public class Update : EndpointBaseAsync
    .WithRequest<UpdateProfileRequest>
    .WithActionResult<ApiResult>
{
    private readonly IIdentityService _identityService;

    public Update(IIdentityService identityService)
    {
        this._identityService = identityService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Update", "", "Updates user details.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var res = await _identityService.UpdateProfile(userId, request.Name, request.Image);
        return Ok(res);
    }
}
using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Data;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<UserDetailsResult>>
{
    private readonly IIdentityService _identityService;

    public Get(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult<UserDetailsResult>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var res = await _identityService.GetUser(userId);
        res.Data.Image = Data.Constants.BlobStorageEndpoint + res.Data.Image;
        return Ok(res);
    }
}
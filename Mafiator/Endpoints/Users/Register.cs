using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
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
[Consumes("application/x-msgpack")]
public class Register : EndpointBaseAsync
    .WithRequest<RegisterUserRequest>
    .WithActionResult<ApiResult>
{
    private readonly IIdentityService _identityService;

    public Register(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Create", "", "Creates a new user.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new ApiResult()
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(s => s.ErrorMessage)),
                StatusCode = ApiResultStatusCode.BadRequest
            });
        }

        var result = await _identityService.Register(request);
        return Ok(result);
    }
}
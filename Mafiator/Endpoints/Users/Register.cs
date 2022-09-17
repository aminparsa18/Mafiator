using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
[Produces("application/x-msgpack")]
public class Register : EndpointBaseAsync
    .WithRequest<RegisterUserRequest>
    .WithActionResult<ApiResult>
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<RegisterUserRequest> _validator;

    public Register(IIdentityService identityService, IValidator<RegisterUserRequest> validator)
    {
        _identityService = identityService;
        _validator = validator;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Create", "", "Creates a new user.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var result = await _identityService.Register(request);
        return Ok(result);
    }
}
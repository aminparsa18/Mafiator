using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Produces("application/x-msgpack")]
public class Refresh : EndpointBaseAsync
    .WithRequest<RefreshTokenRequest>
    .WithActionResult<AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<RefreshTokenRequest> _validator;

    public Refresh(IIdentityService identityService, IValidator<RefreshTokenRequest> validator)
    {
        _identityService = identityService;
        _validator = validator;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Refresh), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<AuthResult>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var result = await _identityService.RefreshToken(request);
        return Ok(result);
    }
}
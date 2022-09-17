using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
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
public class Confirm : EndpointBaseAsync
    .WithRequest<ConfirmPhoneRequest>
    .WithActionResult<AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<ConfirmPhoneRequest> _validator;

    public Confirm(IIdentityService identityService, IValidator<ConfirmPhoneRequest> validator)
    {
        _identityService = identityService;
        _validator = validator;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Confirm", "", "Confirms user phone number.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<AuthResult>> HandleAsync(ConfirmPhoneRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var res = await _identityService.ConfirmPhoneNumber(request.PhoneNo, request.Token);
        return Ok(res);
    }
}
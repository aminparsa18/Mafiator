using Ardalis.ApiEndpoints;
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
[Consumes("application/x-msgpack")]
public class Confirm : EndpointBaseAsync
    .WithRequest<ConfirmPhoneRequest>
    .WithActionResult<AuthResult>
{
    private readonly IIdentityService _identityService;

    public Confirm(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Users.Confirm", "", "Confirms user phone number.")]
    [OpenApiTag("Users Endpoints")]
    public override async Task<ActionResult<AuthResult>> HandleAsync(ConfirmPhoneRequest request, CancellationToken cancellationToken = default)
    {

        if (!ModelState.IsValid)
            return Ok(new AuthResult()
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(s => s.ErrorMessage))
            });
        var res = await _identityService.ConfirmPhoneNumber(request.PhoneNo, request.Token);
        return Ok(res);
    }
}
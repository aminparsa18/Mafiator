using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
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
    private readonly IValidator<UpdateProfileRequest> _validator;

    public Update(IIdentityService identityService, IValidator<UpdateProfileRequest> validator)
    {
        _identityService = identityService;
        _validator = validator;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/users/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Update), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var res = await _identityService.UpdateProfile(userId, request.Name, request.Image);
        return Ok(res);
    }
}
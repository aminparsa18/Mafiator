using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Gems;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Gems;

[Authorize]
[Produces("application/x-msgpack")]
public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<GemResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gems")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Gem Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<GemResult>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResult<IEnumerable<GemResult>>()
        {
            IsSuccess = true,
            Data = await _unitOfWork.Gem.GetAllDto()
        });
    }
}
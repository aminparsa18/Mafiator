using Mafiator.Common.Data.Dtos.Reactions;
using Mafiator.Repository;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Reactions;

[Authorize]
[Produces("application/x-msgpack")]
public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<ReactionResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/reactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Reaction Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<ReactionResult>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResult<IEnumerable<ReactionResult>>()
        {
            IsSuccess = true,
            Data = await _unitOfWork.Reaction.GetAllDtos()
        });
    }
}
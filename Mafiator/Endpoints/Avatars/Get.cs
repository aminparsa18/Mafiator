using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Service.Contracts.Avatars;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Avatars;

[Produces("application/x-memorypack")]
[Consumes("application/x-memorypack")]
public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<AvatarResult>>>
{
    private readonly IAvatarService _avatarService;

    public Get(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }

    [ApiVersion("1.0")]
    [HttpGet(ApiUrls.Avatars)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "summary", Description = "Description", OperationId = nameof(ApiUrls.Avatars), Tags = new[] { "Avatars Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<AvatarResult>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var avatars = await _avatarService.GetAll();
        return Ok(new ApiResult<IEnumerable<AvatarResult>>
        {
            Data = avatars,
            IsSuccess = true
        });
    }
}
using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Avatars;

[Produces("application/x-msgpack")]
[Consumes("application/x-msgpack")]
public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<AvatarResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/avatars")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Avatar.GetAll", "Retrieves all avatars data.")]
    [OpenApiTag("Avatars Endpoints")]
    public override async Task<ActionResult<ApiResult<IEnumerable<AvatarResult>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<AvatarResult> avatars = await _unitOfWork.Avatar.GetAllDtosFast();
        foreach (var avatar in avatars)
        {
            avatar.Name = string.Join(Constants.BlobStorageEndpoint, avatar.Name);
        }
        return Ok(new ApiResult<IEnumerable<AvatarResult>>
        {
            Data = avatars,
            IsSuccess = true
        });
    }
}
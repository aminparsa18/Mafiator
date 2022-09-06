using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Avatars;

public class GetAllAvatarsAsync : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<AvatarDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAvatarsAsync(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //[ApiVersion("1.0")]
    [HttpGet($"test/batch-contents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    //[SwaggerOperation(OperationId = "GetBatchContentsAsync", Tags = new[] { EndpointsTags.UnitsBatches })]
    public override async Task<ActionResult<ApiResult<IEnumerable<AvatarDto>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var avatars = await _unitOfWork.Avatar.GetAllDto();
        foreach (var avatar in avatars)
        {
            avatar.Name = Constants.BlobStorageEndpoint + avatar.Name;
        }
        return Ok(new ApiResult<IEnumerable<AvatarDto>>
        {
            Data = avatars,
            IsSuccess = true
        });
    }
}
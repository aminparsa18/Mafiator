using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.RoomMembers;

[Authorize]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<RoomMemberResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByRoom(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/roommembers/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("RoomMembers.GetByRoom", "", "Retrieves all members in a room.")]
    [OpenApiTag("RoomMembers Endpoints")]
    public override async Task<ActionResult<ApiResult<IEnumerable<RoomMemberResult>>>> HandleAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.RoomMember.GetByRoomFast(roomId);
        foreach (var item in data)
        {
            item.Level = (item.TotalGame / 10) + 1;
            item.Image = Constants.BlobStorageEndpoint + item.Image;
        }

        return Ok(new ApiResult<IEnumerable<RoomMemberResult>>()
        {
            Data = data,
            IsSuccess = true
        });
    }
}
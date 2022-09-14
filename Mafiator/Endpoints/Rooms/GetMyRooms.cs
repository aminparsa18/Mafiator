using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
public class GetMyRooms : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<RoomDetailsResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMyRooms(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.GetMyRooms", "", "Retrieves all of my rooms.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult<RoomDetailsResult>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
        var data = await _unitOfWork.Room.GetMyRoomsFast(userId);
        return Ok(new ApiResult<IEnumerable<RoomDetailsResult>>()
        {
            IsSuccess = true,
            Data = data
        });
    }
}
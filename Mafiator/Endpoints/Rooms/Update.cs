using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using Mafiator.Repository;

namespace Mafiator.Api.Endpoints.Rooms;

[Produces("application/x-msgpack")]
[Consumes("application/x-msgpack")]
public class Update : EndpointBaseAsync
    .WithRequest<UpdateRoomNameRequest>
    .WithActionResult<ApiResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public Update(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPut("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.Create", "", "Updates an existing room.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(UpdateRoomNameRequest request, CancellationToken cancellationToken = default)
    {
        var room = await _unitOfWork.Room.Get(request.RoomId);
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
        if (room.UserId != userId)
            return BadRequest(new ApiResult()
            {
                IsSuccess = false,
                Errors = new[] { "You don't have permission to perform this action." }
            });
        _unitOfWork.Room.Update(room);
        await _unitOfWork.Commit();
        return Ok(new ApiResult()
        {
            IsSuccess = true
        });
    }
}
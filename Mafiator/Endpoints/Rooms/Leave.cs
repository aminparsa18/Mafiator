using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Security.Claims;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

public class Leave : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Leave(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.Join", "", "leaving an existing room.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string code, CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
        var roomId = await _unitOfWork.Room.GetByCode(code);
        if (string.IsNullOrEmpty(roomId))
        {
            return Ok(new ApiResult<string>()
            {
                IsSuccess = false,
                Errors = new[] { "No such room exist" },
                StatusCode = ApiResultStatusCode.NotFound
            });
        }
        var member = await _unitOfWork.RoomMember.Get(r => r.UserId == userId && r.RoomId == Guid.Parse(roomId));
        if (member == null)
            return Ok(new ApiResult<string>()
            {
                IsSuccess = false,
                Errors = new[] { "You are not member of this room" },
                StatusCode = ApiResultStatusCode.Conflict,
                Data = roomId
            });
        _unitOfWork.RoomMember.Remove(member);
        await _unitOfWork.Commit();
        return Ok(new ApiResult<string>()
        {
            IsSuccess = true,
            Data = roomId
        });
    }
}
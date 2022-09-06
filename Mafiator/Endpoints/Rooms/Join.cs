using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

public class Join : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Join(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/join")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.Join", "", "Joining an existing room.")]
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
        var member = await _unitOfWork.Room.IsJoinedFast(userId.ToString(), roomId);
        if (!string.IsNullOrEmpty(member))
            return Ok(new ApiResult<string>()
            {
                IsSuccess = false,
                Errors = new[] { "You are already joined" },
                StatusCode = ApiResultStatusCode.Conflict,
                Data = roomId
            });
        await _unitOfWork.RoomMember.AddFast(new RoomMember()
        {
            RoomId = Guid.Parse(roomId),
            UserId = userId
        });
        return Ok(new ApiResult<string>()
        {
            IsSuccess = true,
            Data = roomId
        });
    }
}
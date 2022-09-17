using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class Leave : EndpointBaseAsync
    .WithRequest<LeaveRoomRequest>
    .WithActionResult<ApiResult>
{
    private readonly IValidator<LeaveRoomRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public Leave(IValidator<LeaveRoomRequest> validator, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.Join", "", "Leaving an existing room.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
        var member = await _unitOfWork.RoomMember.Get(r => r.UserId == userId && r.RoomId == request.RoomId);
        if (member == null)
            return Ok(new ApiResult
            {
                IsSuccess = false,
                Errors = new[] { "You are not member of this room." },
                StatusCode = ApiResultStatusCode.BadRequest
            });
        _unitOfWork.RoomMember.Remove(member);
        await _unitOfWork.Commit();
        return Ok(new ApiResult
        {
            IsSuccess = true,
        });
    }
}
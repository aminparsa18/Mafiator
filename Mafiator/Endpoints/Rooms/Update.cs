using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class Update : EndpointBaseAsync
    .WithRequest<UpdateRoomNameRequest>
    .WithActionResult<ApiResult>
{
    private readonly IValidator<UpdateRoomNameRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public Update(IValidator<UpdateRoomNameRequest> validator, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPut("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Update), Tags = new[] { "Room Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(UpdateRoomNameRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
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
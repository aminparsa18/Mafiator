using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

[Authorize]
[Produces("application/x-msgpack")]
public class JoinById : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ApiResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public JoinById(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms/join/id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.JoinById", "", "Joining an existing room.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await _unitOfWork.RoomMember.AddFast(new RoomMember()
        {
            UserId = Guid.Parse(userId),
            RoomId = id
        });
        return Ok(new ApiResult()
        {
            IsSuccess = true
        });
    }
}
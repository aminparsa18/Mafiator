using Ardalis.ApiEndpoints;
using AutoMapper;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Common.Helpers;
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
public class Create : EndpointBaseAsync
    .WithRequest<RoomCreateRequest>
    .WithActionResult<ApiResult<RoomCreateResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public Create(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.Create", "", "Creates a new room.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult<RoomCreateResult>>> HandleAsync(RoomCreateRequest request, CancellationToken cancellationToken = default)
    {
        var room = _mapper.Map<RoomCreateRequest, Room>(request);
        room.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
        room.Code = RandomHelper.CreateRandomText(8);
        var roomId = await _unitOfWork.Room.AddFast(room);
        await _unitOfWork.RoomMember.AddFast(new RoomMember()
        {
            RoomId = Guid.Parse(roomId.ToString()),
            UserId = room.UserId,
        });
        foreach (var user in request.Users)
        {
            await _unitOfWork.RoomMember.AddFast(new RoomMember()
            {
                RoomId = Guid.Parse(roomId.ToString()),
                UserId = user,
            });
        }
        return Ok(new ApiResult<RoomCreateResult>()
        {
            IsSuccess = true,
            Data = new RoomCreateResult() { Id = room.Id }
        });
    }
}
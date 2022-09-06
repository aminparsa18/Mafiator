using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Common.Helpers;
using Mafiator.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using Mafiator.Common.Data.Dtos.Rooms;

namespace Mafiator.Api.Endpoints.Rooms;

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
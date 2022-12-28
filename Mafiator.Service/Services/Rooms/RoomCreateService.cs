using AutoMapper;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Models;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Rooms;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Rooms;

public class RoomCreateService : IRoomCreateService
{
    private readonly IValidator<RoomCreateRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomCreateService(IValidator<RoomCreateRequest> validator, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResult<RoomCreateResult>> Create(string userId, RoomCreateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ApiResult<RoomCreateResult>
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };
        var room = _mapper.Map<RoomCreateRequest, Room>(request);
        room.UserId = Guid.Parse(userId);
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
        return new ApiResult<RoomCreateResult>
        {
            IsSuccess = true,
            Data = new RoomCreateResult() { Id = room.Id }
        };
    }
}
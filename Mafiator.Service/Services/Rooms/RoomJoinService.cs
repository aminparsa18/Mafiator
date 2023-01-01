using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Entities.Models;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Rooms;
using System;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Rooms;

public class RoomJoinService : IRoomJoinService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomJoinService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<string>> JoinByCode(string userId, string code)
    {
        var roomId = await _unitOfWork.Room.GetByCode(code);
        if (string.IsNullOrEmpty(roomId))
        {
            return new ApiResult<string>
            {
                IsSuccess = false,
                Errors = new[] { "No such room exist" },
                StatusCode = ApiResultStatusCode.NotFound
            };
        }
        var member = await _unitOfWork.Room.IsJoinedFast(userId.ToString(), roomId);
        if (member != null)
            return new ApiResult<string>
            {
                IsSuccess = false,
                Errors = new[] { "You are already joined" },
                StatusCode = ApiResultStatusCode.Conflict,
                Data = roomId
            };
        await _unitOfWork.RoomMember.AddFast(new RoomMember()
        {
            RoomId = Guid.Parse(roomId),
            UserId = Guid.Parse(userId)
        });
        return new ApiResult<string>
        {
            IsSuccess = true,
            Data = roomId
        };
    }

    public async Task<ApiResult> JoinById(string userId, Guid id)
    {
        await _unitOfWork.RoomMember.AddFast(new RoomMember()
        {
            UserId = Guid.Parse(userId),
            RoomId = id
        });
        return new ApiResult
        {
            IsSuccess = true
        };
    }
}
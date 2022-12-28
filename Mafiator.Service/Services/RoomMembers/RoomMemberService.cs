using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Repository;
using Mafiator.Service.Contracts.RoomMembers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.RoomMembers;

public class RoomMemberService : IRoomMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IEnumerable<RoomMemberResult>>> GetByRoom(string roomId)
    {
        var data = await _unitOfWork.RoomMember.GetByRoomFast(roomId);
        foreach (var item in data)
        {
            item.Level = (item.TotalGame / 10) + 1;
            item.Image = string.Join(Data.Constants.BlobStorageEndpoint, item.Image);
        }

        return new ApiResult<IEnumerable<RoomMemberResult>>
        {
            Data = data,
            IsSuccess = true
        };
    }
}
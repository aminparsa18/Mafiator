using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Rooms;

public interface IRoomLeaveService
{
    Task<ApiResult> Leave(string userId, LeaveRoomRequest request);
}
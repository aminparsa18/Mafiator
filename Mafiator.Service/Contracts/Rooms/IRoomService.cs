using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Rooms;

public interface IRoomService
{
    Task<ApiResult<RoomDetailsResult>> GetDetails(string roomId);

    Task<ApiResult<IEnumerable<RoomDetailsResult>>> GetMyRooms(string userId);

    Task<ApiResult<string>> IsJoined(string userId, string roomId);
}
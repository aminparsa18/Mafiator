using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Rooms;

public interface IRoomUpdateService
{
    Task<ApiResult> Update(string userId, UpdateRoomNameRequest request);
}
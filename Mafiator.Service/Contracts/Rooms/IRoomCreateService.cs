using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Rooms;

public interface IRoomCreateService
{
    Task<ApiResult<RoomCreateResult>> Create(string userId, RoomCreateRequest request);
}
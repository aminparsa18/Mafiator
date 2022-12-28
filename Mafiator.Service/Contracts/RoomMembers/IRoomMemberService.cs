using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.RoomMembers;

public interface IRoomMemberService
{
    Task<ApiResult<IEnumerable<RoomMemberResult>>> GetByRoom(string roomId);
}
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.RoomMembers;

public interface IRoomMemberCreateService
{
    Task<ApiResult> Create(NewMembersRequest request);
}
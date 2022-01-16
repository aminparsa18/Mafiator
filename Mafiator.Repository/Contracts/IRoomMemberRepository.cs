using Mafiator.Data.Dtos.Room;
using Mafiator.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IRoomMemberRepository:IRepository<RoomMember>
    {
        Task<IEnumerable<Guid>> FindInRoom(Guid roomId, Guid userId);
        Task<List<RoomMemberDto>> GetByRoom(Guid roomId);
        Task<IEnumerable<RoomMemberDto>> GetByRoomFast(string roomId);
    }
}

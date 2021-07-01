using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IRoomMemberRepository:IRepository<RoomMember>
    {
        Task<IEnumerable<Ulid>> FindInRoom(Ulid roomId, Ulid userId);
        Task<List<RoomMemberDto>> GetByRoom(Ulid roomId);
        Task<IEnumerable<RoomMemberDto>> GetByRoomFast(string roomId);
    }
}

using Mafiator.Data.Dtos.Room;
using Mafiator.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IRoomRepository:IRepository<Room>
    {
        Task<IEnumerable<RoomDto>> GetRoomFast(string roomId);
        Task<List<RoomDto>> GetDtoPage(int skip);
        Task<IEnumerable<RoomDto>> GetDtoPageFast(int skip);
        Task<RoomDto> GetRoom(string roomId);
        Task<List<RoomDto>> GetMyRooms(Guid userId);
        Task<IEnumerable<RoomDto>> GetMyRoomsFast(Guid userId);
        Task<RoomMember> IsJoined(Guid userId, Guid roomId);
        Task<string> IsJoinedFast(string userId, string roomId);
        Task<string> GetByCode(string code);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IRoomRepository:IRepository<Room>
    {
        Task<IEnumerable<RoomDto>> GetRoomFast(string roomId);
        Task<List<RoomDto>> GetDtoPage(int skip);
        Task<IEnumerable<RoomDto>> GetDtoPageFast(int skip);
        Task<RoomDto> GetRoom(string roomId);
        Task<List<RoomDto>> GetMyRooms(Ulid userId);
        Task<IEnumerable<RoomDto>> GetMyRoomsFast(Ulid userId);
        Task<RoomMember> IsJoined(Ulid userId, Ulid roomId);
        Task<string> IsJoinedFast(string userId, string roomId);
        Task<string> GetByCode(string code);
    }
}

using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.Room;
using Mafiator.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IGameRepository:IRepository<Game>
    {
        Task<IEnumerable<RoomGameDto>> GetByRoom(string roomId);
        Task<WaitingGameDto> GetWaitingGameByGame(Guid gameId);
        Task<WaitingGameDto> GetWaitingGameByRoom(Guid roomId);
        Task<List<GameDto>> GetAvailables();
        Task<string> IsJoinedFast(string userId, string gameId);
        Task<string> IsAlreadyPlaying(string roomId);
        Task<int> StartGame(string gameId);
        Task<int> MafiaWin(string gameId);
        Task<int> CitizenWin(string gameId);
    }
}

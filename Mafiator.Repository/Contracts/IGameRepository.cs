using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IGameRepository:IRepository<Game>
    {
        Task<IEnumerable<RoomGameDto>> GetByRoom(string roomId);
        Task<WaitingGameDto> GetWaitingGameByGame(Ulid gameId);
        Task<WaitingGameDto> GetWaitingGameByRoom(Ulid roomId);
        Task<List<GameDto>> GetAvailables();
        Task<string> IsJoinedFast(string userId, string gameId);
        Task<string> IsAlreadyPlaying(string roomId);
        Task<int> StartGame(string gameId);
        Task<int> MafiaWin(string gameId);
        Task<int> CitizenWin(string gameId);
    }
}

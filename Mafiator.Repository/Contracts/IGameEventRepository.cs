using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IGameEventRepository:IRepository<GameEvent>
    {
        Task<IEnumerable<GameEventStatusDto>> GetByGame(string gameId);
        Task<int> Validate(string gameId);
    }
}

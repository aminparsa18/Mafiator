using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IGameEventRepository:IRepository<GameEvent>
    {
        Task<IEnumerable<GameEventStatusDto>> GetByGame(string gameId);
        Task<int> Validate(string gameId);
    }
}

using Mafiator.Data.Dtos.Game;
using Mafiator.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IReactionRepository:IRepository<Reaction>
    {
        Task<IEnumerable<ReactionDto>> GetAllDtos();
    }
}

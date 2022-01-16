using Mafiator.Data.Dtos;
using Mafiator.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IGemRepository:IRepository<Gem>
    {
        Task<IEnumerable<GemDto>> GetAllDto();
    }
}

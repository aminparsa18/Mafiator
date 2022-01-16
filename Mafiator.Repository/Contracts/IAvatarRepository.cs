using Mafiator.Data.Dtos;
using Mafiator.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IAvatarRepository : IRepository<Avatar>
    {
        Task<IEnumerable<AvatarDto>> GetAllDto();
    }
}
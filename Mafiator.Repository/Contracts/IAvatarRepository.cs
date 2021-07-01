using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
   public interface IAvatarRepository:IRepository<Avatar>
   {
       Task<IEnumerable<AvatarDto>> GetAllDto();
   }
}

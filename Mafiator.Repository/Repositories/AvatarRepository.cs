using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using RepoDb;

namespace Mafiator.Repository.Repositories
{
   public class AvatarRepository:Repository<Avatar>,IAvatarRepository
    {
        public AvatarRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }

        public async Task<IEnumerable<AvatarDto>> GetAllDto()
        {
            //return await connection.ExecuteQueryAsync<AvatarDto>("SELECT Name FROM [Avatar]",cacheKey:"ActiveAvatars",cache:CacheFactory.GetCache());
            return await connection.ExecuteQueryAsync<AvatarDto>("SELECT Name FROM [Avatar]");
        }
    }
}

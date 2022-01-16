using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using RepoDb;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class GemRepository:Repository<Gem>,IGemRepository
    {
        public GemRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<IEnumerable<GemDto>> GetAllDto()
        {
            return Connection.ExecuteQueryAsync<GemDto>("SELECT Id,Count,Price,Image FROM [Gem] ORDER BY Count");
        }
    }
}

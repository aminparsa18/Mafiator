using Mafiator.Data;
using Mafiator.Data.Dtos.Game;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using RepoDb;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class ReactionRepository:Repository<Reaction>,IReactionRepository
    {
        public ReactionRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<IEnumerable<ReactionDto>> GetAllDtos()
        {
            return Connection.ExecuteQueryAsync<ReactionDto>(@"SELECT [r].[Id], [r].[Title],[r].[Image]
            FROM [Reaction] AS [r]");
        }
    }
}

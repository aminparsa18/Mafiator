using Mafiator.Data;
using Mafiator.Data.Dtos.Vote;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using RepoDb;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class VoteRepository: Repository<Vote>, IVoteRepository
    {
        public VoteRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<int> Validate(string gameId)
        {
            return Connection.ExecuteNonQueryAsync("UPDATE [Vote] SET [IsValidated] = 1 WHERE [GameId] = @gameId", new { gameId });

        }
        public Task<IEnumerable<VoteValidateDto>> GetNonValidatedTargets(string gameId)
        {
            return Connection.ExecuteQueryAsync<VoteValidateDto>("SELECT v.[TargetId] FROM [Vote] v WHERE v.[GameId] = @gameId AND v.[IsValidated] = 0", new { gameId },cacheKey:$"Targets-{gameId}",cache:CacheFactory.GetCache());
        }

        public Task<IEnumerable<VoteStatusDto>> GetVoteStatus(string gameId)
        {
            return Connection.ExecuteQueryAsync<VoteStatusDto>("SELECT v.[TargetId],v.[VoterId] FROM [Vote] v WHERE v.[GameId] = @gameId AND v.[IsValidated] = 0", new { gameId }, cacheKey: $"Votes-{gameId}", cache: CacheFactory.GetCache(),cacheItemExpiration:1);
        }
    }
}

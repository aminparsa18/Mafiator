using Mafiator.Data.Dtos.Vote;
using System.Collections.Generic;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class VoteRepository : BaseRepository<Vote>, IVoteRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VoteRepository"/> class.
    /// </summary>
    public VoteRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<int> Validate(string gameId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [Vote] SET [IsValidated] = 1 WHERE [GameId] = @gameId", new { gameId });

    }

    /// <inheritdoc/>
    public Task<IEnumerable<VoteValidateDto>> GetNonValidatedTargets(string gameId)
    {
        return Connection.ExecuteQueryAsync<VoteValidateDto>("SELECT v.[TargetId] FROM [Vote] v WHERE v.[GameId] = @gameId AND v.[IsValidated] = 0", new { gameId }, cacheKey: $"Targets-{gameId}", cache: CacheFactory.GetCache());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<VoteStatusDto>> GetVoteStatus(string gameId)
    {
        return Connection.ExecuteQueryAsync<VoteStatusDto>("SELECT v.[TargetId],v.[VoterId] FROM [Vote] v WHERE v.[GameId] = @gameId AND v.[IsValidated] = 0", new { gameId }, cacheKey: $"Votes-{gameId}", cache: CacheFactory.GetCache(), cacheItemExpiration: 1);
    }
}
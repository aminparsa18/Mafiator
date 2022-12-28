using Mafiator.Common.Data.Dtos.Reactions;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public sealed class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactionRepository"/> class.
    /// </summary>
    public ReactionRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<ReactionResult>> GetAllDtos()
    {
        return Connection.ExecuteQueryAsync<ReactionResult>(@"SELECT [r].[Id], [r].[Title],[r].[Image]
            FROM [Reaction] AS [r]");
    }
}
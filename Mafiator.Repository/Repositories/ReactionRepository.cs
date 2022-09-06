using Mafiator.Data.Dtos.Game;
using System.Collections.Generic;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactionRepository"/> class.
    /// </summary>
    public ReactionRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<ReactionDto>> GetAllDtos()
    {
        return Connection.ExecuteQueryAsync<ReactionDto>(@"SELECT [r].[Id], [r].[Title],[r].[Image]
            FROM [Reaction] AS [r]");
    }
}
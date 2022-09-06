using Mafiator.Data.Dtos.GameEvent;
using System.Collections.Generic;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class GameEventRepository : BaseRepository<GameEvent>, IGameEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameEventRepository"/> class.
    /// </summary>
    public GameEventRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<GameEventStatusDto>> GetByGame(string gameId)
    {
        return Connection.ExecuteQueryAsync<GameEventStatusDto>("SELECT [g].[MemberId],[g].[EventType] FROM [GameEvent] g WHERE g.[GameId] = @gameId AND g.[IsValidated] = 0", new { gameId });
    }

    /// <inheritdoc/>
    public Task<int> Validate(string gameId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [GameEvent] SET [IsValidated] = 1 WHERE [GameId] = @gameId", new { gameId });
    }
}
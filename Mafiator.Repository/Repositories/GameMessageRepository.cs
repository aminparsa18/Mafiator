using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class GameMessageRepository : BaseRepository<GameMessage>, IGameMessageRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameMessageRepository"/> class.
    /// </summary>
    public GameMessageRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }
}
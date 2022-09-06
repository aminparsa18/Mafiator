using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class EventJoinRepository : BaseRepository<EventJoin>, IEventJoinRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventJoinRepository"/> class.
    /// </summary>
    public EventJoinRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }
}
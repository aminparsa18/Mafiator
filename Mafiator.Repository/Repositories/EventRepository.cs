using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public sealed class EventRepository : BaseRepository<Event>, IEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventRepository"/> class.
    /// </summary>
    public EventRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }
}
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class GameViolationReportRepository : BaseRepository<GameViolationReport>, IGameViolationReportRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameViolationReportRepository"/> class.
    /// </summary>
    public GameViolationReportRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }
}
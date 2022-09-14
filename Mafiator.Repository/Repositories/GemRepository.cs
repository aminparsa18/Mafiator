using Mafiator.Common.Data.Dtos.Gems;
using System.Collections.Generic;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public sealed class GemRepository : BaseRepository<Gem>, IGemRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GemRepository"/> class.
    /// </summary>
    public GemRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<GemResult>> GetAllDto()
    {
        return Connection.ExecuteQueryAsync<GemResult>("SELECT Id,Count,Price,Image FROM [Gem] ORDER BY Count");
    }
}
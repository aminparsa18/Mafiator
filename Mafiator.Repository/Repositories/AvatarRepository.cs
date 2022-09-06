using Mafiator.Common.Data.Dtos.Avatars;
using System.Collections.Generic;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class AvatarRepository : BaseRepository<Avatar>, IAvatarRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvatarRepository"/> class.
    /// </summary>
    public AvatarRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AvatarResult>> GetAllDto()
    {
        //return await connection.ExecuteQueryAsync<AvatarDto>("SELECT Name FROM [Avatar]",cacheKey:"ActiveAvatars",cache:CacheFactory.GetCache());
        return await Connection.ExecuteQueryAsync<AvatarResult>("SELECT Name FROM [Avatar]");
    }
}
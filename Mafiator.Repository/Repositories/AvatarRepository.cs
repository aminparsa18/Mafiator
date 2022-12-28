using Mafiator.Common.Data.Dtos.Avatars;
using System.Data;
using System.Linq;

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
    public Task<List<AvatarResult>> GetAllDtos()
    {
        return Context.Avatar.AsNoTracking().Select(s => new AvatarResult
        {
            Name = s.Name
        }).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AvatarResult>> GetAllDtosFast()
    {
        return await Connection.ExecuteQueryAsync<AvatarResult>("SELECT Name FROM [Avatar]");
    }
}
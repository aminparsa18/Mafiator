using Mafiator.Data.Dtos.User;
using System.Collections.Generic;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
    /// </summary>
    public RefreshTokenRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<RefreshTokenDto>> GetByToken(string refreshToken)
    {
        return Connection.ExecuteQueryAsync<RefreshTokenDto>(@"SELECT [r].[Id], [r].[ExpirationDate], [r].[IsInvalidated], [r].[IsUsed], [r].[JwtId]
            FROM[dbo].[RefreshToken] AS[r]
            WHERE [r].[Token] = @refreshToken", new { refreshToken });
    }

    /// <inheritdoc/>
    public Task<int> SetUsed(string id)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [RefreshToken] SET [IsUsed] = 1 WHERE [Id] = @id", new { id });
    }
}
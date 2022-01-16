using Mafiator.Data;
using Mafiator.Data.Dtos.User;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using RepoDb;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class RefreshTokenRepository:Repository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<IEnumerable<RefreshTokenDto>> GetByToken(string refreshToken)
        {
            return Connection.ExecuteQueryAsync<RefreshTokenDto>(@"SELECT [r].[Id], [r].[ExpirationDate], [r].[IsInvalidated], [r].[IsUsed], [r].[JwtId]
            FROM[dbo].[RefreshToken] AS[r]
            WHERE [r].[Token] = @refreshToken", new{refreshToken});
        }

        public Task<int> SetUsed(string id)
        {
           return Connection.ExecuteNonQueryAsync("UPDATE [RefreshToken] SET [IsUsed] = 1 WHERE [Id] = @id", new {id});
        }
    }
}

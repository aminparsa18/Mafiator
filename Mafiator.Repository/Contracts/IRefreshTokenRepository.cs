using Mafiator.Data.Dtos.User;
using Mafiator.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IRefreshTokenRepository:IRepository<RefreshToken>
    {
        Task<IEnumerable<RefreshTokenDto>> GetByToken(string refreshToken);
        Task<int> SetUsed(string id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IRefreshTokenRepository:IRepository<RefreshToken>
    {
        Task<IEnumerable<RefreshTokenDto>> GetByToken(string refreshToken);
        Task<int> SetUsed(string id);
    }
}

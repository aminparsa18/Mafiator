using Mafiator.Data.Dtos.User;
using System.Collections.Generic;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle refresh token data.
/// </summary>
public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    /// <summary>
    /// Retrieves token by refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <returns>List of tokens.</returns>
    Task<IEnumerable<RefreshTokenDto>> GetByToken(string refreshToken);

    /// <summary>
    /// Update flag for token to be used.
    /// </summary>
    /// <param name="refreshTokenId">Refresh token identifier.</param>
    /// <returns>Number of affected rows.</returns>
    Task<int> SetUsed(string refreshTokenId);
}
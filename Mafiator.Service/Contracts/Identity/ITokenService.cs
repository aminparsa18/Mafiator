using Mafiator.Common.Api;
using Mafiator.Entities.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mafiator.Service.Contracts.Identity
{
    public interface ITokenService
    {
        GenerateTokenResult GenerateAccessToken(User user,List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

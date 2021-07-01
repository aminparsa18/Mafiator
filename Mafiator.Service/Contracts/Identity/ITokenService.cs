using System.Collections.Generic;
using System.Security.Claims;
using Mafiator.Common.Api;
using Mafiator.Entities.Identity;

namespace Mafiator.Service.Contracts.Identity
{
   public interface ITokenService
    {
        GenerateTokenResult GenerateAccessToken(User user,List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

using Mafiator.Common.Api;
using Mafiator.Entities.Identity;
using Mafiator.Service.Contracts.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Jwt = Mafiator.Data.Jwt;

namespace Mafiator.Service.Services.Identity;

public class TokenService : ITokenService
{
    private readonly Jwt _jwt;
    private readonly TokenValidationParameters _tokenValidationParameters;
    public TokenService(IOptions<Jwt> jwt, TokenValidationParameters tokenValidationParameters)
    {
        _jwt = jwt.Value;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public GenerateTokenResult GenerateAccessToken(User user, List<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwt.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwt.TokenLifeTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new GenerateTokenResult()
        {
            JwtId = token.Id,
            Token = tokenHandler.WriteToken(token)
        };
    }

    public string GenerateRefreshToken()
    {
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = _tokenValidationParameters.Clone();
        tokenValidationParameters.ValidateLifetime = false;
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        return securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)
            ? throw new SecurityTokenException("Invalid token")
            : principal;
    }
}
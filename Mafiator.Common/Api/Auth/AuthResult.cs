using MessagePack;

namespace Mafiator.Common.Api.Auth;

/// <summary>
/// Api result for authentication api calls.
/// </summary>
[MessagePackObject]
public class AuthResult : ApiResult
{
    /// <summary>
    /// Jwt token.
    /// </summary>
    [Key(3)] public string Token { get; set; }

    /// <summary>
    /// Refresh token needed for refresh expired token.
    /// </summary>
    [Key(4)] public string RefreshToken { get; set; }
}
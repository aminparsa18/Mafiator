using MessagePack;

namespace Mafiator.Common.Api.Auth;

/// <summary>
/// Request to generate new token with refresh token.
/// </summary>
[MessagePackObject()]
public class RefreshTokenRequest
{
    /// <summary>
    /// Jwt expired token.
    /// </summary>
    [Key(0)] public string Token { get; }

    /// <summary>
    /// Refresh token needed for refresh expired token.
    /// </summary>
    [Key(1)] public string RefreshToken { get; }
}
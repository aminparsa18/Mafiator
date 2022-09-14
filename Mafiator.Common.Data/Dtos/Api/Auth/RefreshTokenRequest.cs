using MessagePack;

namespace Mafiator.Common.Data.Dtos.Api.Auth
{
    /// <summary>
    /// Request to generate new token with refresh token.
    /// </summary>
    [MessagePackObject()]
    public sealed class RefreshTokenRequest
    {
        /// <summary>
        /// Jwt expired token.
        /// </summary>
        [Key(0)] public string Token { get; set; }

        /// <summary>
        /// Refresh token needed for refresh expired token.
        /// </summary>
        [Key(1)] public string RefreshToken { get; set; }
    }
}
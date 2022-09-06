using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

/// <summary>
/// Refresh token.
/// </summary>
public class RefreshToken:BaseEntity
{
    /// <summary>
    /// Jwt token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Jwt key identifier.
    /// </summary>
    public string JwtId { get; set; }

    /// <summary>
    /// Token expiration date.
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Indicates token is already used.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Indicates token is already invalidated.
    /// </summary>
    public bool IsInvalidated { get; set; }

    /// <summary>
    /// User key identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }
}
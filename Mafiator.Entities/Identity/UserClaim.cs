using Microsoft.AspNetCore.Identity;
using System;

namespace Mafiator.Entities.Identity;

/// <summary>
/// User claim.
/// </summary>
public class UserClaim : IdentityUserClaim<Guid>
{
    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }
}
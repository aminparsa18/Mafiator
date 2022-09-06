using Microsoft.AspNetCore.Identity;
using System;

namespace Mafiator.Entities.Identity;

/// <summary>
/// User role.
/// </summary>
public class UserRole : IdentityUserRole<Guid>
{
    /// <summary>
    /// Role.
    /// </summary>
    public virtual Role Role { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }
}
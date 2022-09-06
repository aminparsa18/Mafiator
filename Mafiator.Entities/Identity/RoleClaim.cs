using Microsoft.AspNetCore.Identity;
using System;

namespace Mafiator.Entities.Identity;

/// <summary>
/// Role claim.
/// </summary>
public class RoleClaim : IdentityRoleClaim<Guid>
{
    /// <summary>
    /// Role.
    /// </summary>
    public virtual Role Role { get; set; }
}
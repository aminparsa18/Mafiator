using System;
using Microsoft.AspNetCore.Identity;

namespace Mafiator.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<Ulid>
    {
        public virtual Role Role { get; set; }
    }
}
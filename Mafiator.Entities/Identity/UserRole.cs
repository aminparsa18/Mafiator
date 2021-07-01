using System;
using Microsoft.AspNetCore.Identity;

namespace Mafiator.Entities.Identity
{
    public class UserRole : IdentityUserRole<Ulid>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
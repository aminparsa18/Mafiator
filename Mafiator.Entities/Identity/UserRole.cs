using Microsoft.AspNetCore.Identity;
using System;

namespace Mafiator.Entities.Identity
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
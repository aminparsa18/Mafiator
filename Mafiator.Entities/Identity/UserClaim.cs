using System;
using Microsoft.AspNetCore.Identity;

namespace Mafiator.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<Ulid>
    {
        public virtual User User { get; set; }
    }
}
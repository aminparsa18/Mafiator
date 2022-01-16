using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities.Identity
{
    public sealed class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }
        public Role(string name) : base(name)
        {
            Name = name;
        }
        public ICollection<UserRole> Users { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
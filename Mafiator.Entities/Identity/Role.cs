using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Mafiator.Entities.Identity
{
    public sealed class Role : IdentityRole<Ulid>
    {
        public Role()
        {
        }
        public Role(string name) : base(name)
        {
            Name = name;
        }
        public string PersianCaption { get; set; }

        public ICollection<UserRole> Users { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
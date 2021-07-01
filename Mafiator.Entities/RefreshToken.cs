using System;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInvalidated { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid UserId { get; set; }
        public virtual User User { get; set; }
    }
}

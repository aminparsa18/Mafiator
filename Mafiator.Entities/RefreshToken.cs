using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInvalidated { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}

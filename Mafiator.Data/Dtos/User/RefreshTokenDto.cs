using System;

namespace Mafiator.Data.Dtos.User
{
    public class RefreshTokenDto
    {
        public string Id { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInvalidated { get; set; }
    }
}

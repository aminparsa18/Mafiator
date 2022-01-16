using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }
        public string CountryCode { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public bool IsSuspended { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public int Score { get; set; }

        public virtual ICollection<EventJoin> EventJoin { get; set; }
        public virtual ICollection<GameMember> GameMember { get; set; }
        public virtual ICollection<GameMessage> GameMessage { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage{ get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
        public virtual ICollection<GameViolationReport> Reporter { get; set; }
        public virtual ICollection<GameViolationReport> Reported { get; set; }
        public virtual ICollection<Room> Room { get; set; }
        public virtual ICollection<RoomMember> RoomMember { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
    }
}
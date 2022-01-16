using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities
{
    public class Room:BaseEntity
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public bool IsPrivate { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Game> Game { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage{ get; set; }
        public virtual ICollection<RoomMember> RoomMember { get; set; }
    }
}

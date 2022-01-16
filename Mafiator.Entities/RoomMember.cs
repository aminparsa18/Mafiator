using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities
{
    public class RoomMember:BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}

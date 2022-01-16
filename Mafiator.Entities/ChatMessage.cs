using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities
{
    public class ChatMessage:BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public GameMessageType MessageType { get; set; }
        public string Content { get; set; }
        public virtual Room Room{ get; set; }
        public virtual User User { get; set; }
    }
}

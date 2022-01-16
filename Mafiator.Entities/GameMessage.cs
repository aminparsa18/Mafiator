using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities
{
    public class GameMessage:BaseEntity
    {
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public GameMessageType MessageType { get; set; }
        public string Content { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}

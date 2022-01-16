using Mafiator.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities
{
    public class Game:BaseEntity
    {
        public DateTime StartDate { get; set; }
        public Guid RoomId { get; set; }
        public GameStatus Status { get; set; }
        public short Capacity { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<GameEvent> GameEvent{ get; set; }
        public virtual ICollection<GameMember> GameMember { get; set; }
        public virtual ICollection<GameMessage> GameMessage { get; set; }
        public virtual ICollection<Vote> Vote{ get; set; }
        public virtual ICollection<GameViolationReport> Report{ get; set; }
    }
}

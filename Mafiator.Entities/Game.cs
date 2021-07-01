using System;
using System.Collections.Generic;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Enums;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class Game:BaseEntity
    {
        public DateTime StartDate { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid RoomId { get; set; }
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

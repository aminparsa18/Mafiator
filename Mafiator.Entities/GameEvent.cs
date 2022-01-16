using Mafiator.Entities.Enums;
using System;

namespace Mafiator.Entities
{
    public class GameEvent:BaseEntity
    {
        public Guid GameId { get; set; }
        public bool IsValidated{ get; set; }
        public Guid MemberId { get; set; }
        public GameEventType EventType { get; set; }

        public virtual Game Game { get; set; }
        public virtual GameMember Member{ get; set; }
    }
}

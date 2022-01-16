using Mafiator.Entities.Enums;
using MessagePack;
using System;

namespace Mafiator.Data.Dtos.GameEvent
{
    [MessagePackObject()]
    public class GameEventDto
    {
        [Key(0)]
        public Guid GameId { get; set; }
        [Key(1)]
        public Guid MemberId { get; set; }
        [Key(2)]
        public GameEventType EventType { get; set; }
    }
}

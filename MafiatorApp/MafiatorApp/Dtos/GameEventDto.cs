using System;
using MafiatorApp.Enums;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
    public class GameEventDto
    {
        [Key(0)]
        public Ulid GameId { get; set; }
        [Key(1)]
        public Ulid MemberId { get; set; }
        [Key(2)]
        public GameEventType EventType { get; set; }
    }
}

using Mafiator.Entities.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos.GameEvent
{
    [MessagePackObject()]
    public class GameEventStatusDto
    {
        [Key(0)]
        public string MemberId { get; set; }
        [Key(1)]
        public GameEventType EventType { get; set; }
    }
}

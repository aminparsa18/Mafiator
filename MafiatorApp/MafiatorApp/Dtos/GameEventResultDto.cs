using MafiatorApp.Enums;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
    public class GameEventResultDto
    {
        [Key(0)]
        public string MemberId { get; set; }
        [Key(1)]
        public GameEventType EventType { get; set; }
        [Key(2)]
        public string Image { get; set; }
        [Key(3)]
        public string Status { get; set; }
        [Key(4)]
        public string Description { get; set; }
        [Key(5)]
        public string DisplayName { get; set; }

    }
}

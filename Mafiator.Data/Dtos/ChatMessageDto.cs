using Mafiator.Entities.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class ChatMessageDto
    {
        [Key(0)]
        public string Content { get; set; }
        [Key(1)]
        public string Image { get; set; }
        [Key(2)]
        public string DisplayName { get; set; }
        [Key(3)]
        public GameMessageType Type { get; set; }
    }
}

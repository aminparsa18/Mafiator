using Mafiator.Entities.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class PlayerDto
    {
        [Key(0)]
        public string MemberId { get; set; }
        [Key(1)]
        public string UserId { get; set; }
        [Key(2)]
        public GameRole Role{ get; set; }
        [Key(3)]
        public PlayerStatus Status{ get; set; }
    }
}
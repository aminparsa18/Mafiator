using MafiatorApp.Enums;
using MessagePack;

namespace MafiatorApp.Dtos.Game
{
    [MessagePackObject()]
    public class PlayerRoleDto
    {
        [Key(0)]
        public GameRole Role { get; set; }
        [Key(1)]
        public string MemberId { get; set; }
    }
}

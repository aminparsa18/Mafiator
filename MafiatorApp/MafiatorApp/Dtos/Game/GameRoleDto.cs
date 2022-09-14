using Mafiator.Common.Data.Enums;
using MessagePack;

namespace MafiatorApp.Dtos.Game
{
    [MessagePackObject]
    public class GameRoleDto
    {
        [Key(0)]
        public GameRole? Role { get; set; }

        [Key(1)]
        public short Count { get; set; }
    }
}

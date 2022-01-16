using MafiatorApp.Enums;
using MessagePack;

namespace MafiatorApp.Dtos.Game
{
   [MessagePackObject()]
   public class GameMemberDto
    {
        [Key(0)]
        public string Id{ get; set; }
        [Key(1)]
        public string DisplayName { get; set; }
        [Key(2)]
        public string Image { get; set; }
        [Key(3)]
        public int Score { get; set; }
        [Key(4)]
        public PlayerStatus Status { get; set; }
    }
}

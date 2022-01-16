using MessagePack;

namespace MafiatorApp.Dtos.Game
{
    [MessagePackObject()]
    public class WaitingGameMemberDto
    {
        [Key(0)]
        public string UserId { get; set; }
        [Key(1)]
        public string DisplayName { get; set; }
        [Key(2)]
        public string Image { get; set; }
        [Key(3)]
        public int Score { get; set; }
    }
}

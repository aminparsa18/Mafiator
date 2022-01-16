using MessagePack;

namespace MafiatorApp.Dtos.User
{
    [MessagePackObject()]
    public class UserStatusDto
    {
        [Key(0)]
        public double MafiaWin { get; set; }
        [Key(1)]
        public double CitizenWin { get; set; }
        [Key(2)]
        public double TotalWin { get; set; }
    }
}

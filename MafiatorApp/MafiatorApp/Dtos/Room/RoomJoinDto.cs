using MessagePack;

namespace MafiatorApp.Dtos.Room
{
    [MessagePackObject()]
   public class RoomJoinDto
    {
        [Key(0)]
        public string Code{ get; set; }
    }
}

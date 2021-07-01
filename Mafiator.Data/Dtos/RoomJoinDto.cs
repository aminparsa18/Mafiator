using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
   public class RoomJoinDto
    {
        [Key(0)]
        public string Code{ get; set; }
    }
}

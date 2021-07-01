using System;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
   public class RoomJoinDto
    {
        [Key(0)]
        public string Code{ get; set; }
    }
}

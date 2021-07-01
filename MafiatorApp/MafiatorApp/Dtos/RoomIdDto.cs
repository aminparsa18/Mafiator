using System;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject]
   public class RoomIdDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
    }
}

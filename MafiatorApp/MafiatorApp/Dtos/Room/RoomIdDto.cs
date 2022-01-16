using MessagePack;
using System;

namespace MafiatorApp.Dtos.Room
{
    [MessagePackObject]
   public class RoomIdDto
    {
        [Key(0)]
        public Guid Id { get; set; }
    }
}

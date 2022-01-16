using MessagePack;
using System;

namespace Mafiator.Data.Dtos.Room
{
   [MessagePackObject]
   public class UpdateRoomImageDto
    {
        [Key(0)]
        public Guid RoomId { get; set; }
        [Key(1)]
        public string Name { get; set; }
    }
}

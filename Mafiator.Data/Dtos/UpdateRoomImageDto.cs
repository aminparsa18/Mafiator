using System;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject]
   public class UpdateRoomImageDto
    {
        [Key(0)]
        public Ulid RoomId { get; set; }
        [Key(1)]
        public string Name { get; set; }
    }
}

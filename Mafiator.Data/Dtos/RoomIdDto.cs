using System;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject]
   public class RoomIdDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
    }
}

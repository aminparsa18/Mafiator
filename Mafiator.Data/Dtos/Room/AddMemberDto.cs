using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Room
{
   [MessagePackObject()]
   public record AddMemberDto
    {
        [Key(0)]
        public Guid RoomId { get; set; }
        [Key(1)]
        public List<Guid> Users { get; set; }
    }
}

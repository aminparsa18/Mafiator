using MessagePack;
using System;
using System.Collections.Generic;

namespace MafiatorApp.Dtos.Game
{
   [MessagePackObject()]
   public class AddMemberDto
    {
        [Key(0)]
        public Guid RoomId { get; set; }
        [Key(1)]
        public List<Guid> Users { get; set; }
    }
}

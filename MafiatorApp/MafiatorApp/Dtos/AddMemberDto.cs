using System;
using System.Collections.Generic;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
   public class AddMemberDto
    {
        [Key(0)]
        public Ulid RoomId { get; set; }
        [Key(1)]
        public List<Ulid> Users { get; set; }
    }
}

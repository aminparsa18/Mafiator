using MessagePack;
using System;
using System.Collections.Generic;

namespace MafiatorApp.Dtos.Room
{
    [MessagePackObject]
   public class RoomCreateDto
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public bool IsPrivate { get; set; }
        [Key(2)]
        public List<Guid> Users { get; set; }
        [Key(3)]
        public string Country{ get; set; }
    }
}

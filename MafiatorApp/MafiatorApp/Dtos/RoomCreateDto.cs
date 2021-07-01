using System;
using System.Collections.Generic;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject]
   public class RoomCreateDto
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public bool IsPrivate { get; set; }
        [Key(2)]
        public List<Ulid> Users { get; set; }
        [Key(3)]
        public string Country{ get; set; }
    }
}

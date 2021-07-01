using System;
using System.Collections.Generic;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject]
   public class GameCreateDto
    {
        [Key(0)]
        public DateTime StartDate { get; set; }
        [Key(1)]
        public List<GameRoleDto> Roles { get; set; }
        [Key(2)]
        public Ulid RoomId { get; set; }
    }
}

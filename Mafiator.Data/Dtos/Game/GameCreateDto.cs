using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Game
{
   [MessagePackObject]
   public class GameCreateDto
    {
        [Key(0)]
        public DateTime StartDate { get; set; }
        [Key(1)]
        public List<GameRoleDto> Roles { get; set; }
        [Key(2)]
        public Guid RoomId { get; set; }
    }
}

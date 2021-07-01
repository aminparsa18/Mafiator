using System;
using Mafiator.Entities.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject]
   public class RoomGameDto
    {
        [Key(0)]
        public DateTime StartDate { get; set; }
        [Key(1)]
        public GameStatus Status { get; set; }
    }
}

using System;
using MafiatorApp.Enums;
using MessagePack;

namespace MafiatorApp.Dtos
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

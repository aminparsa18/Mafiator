using MafiatorApp.Enums;
using MessagePack;
using System;

namespace MafiatorApp.Dtos.Room
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

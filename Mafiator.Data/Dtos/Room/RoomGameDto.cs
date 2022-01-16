using Mafiator.Entities.Enums;
using MessagePack;
using System;

namespace Mafiator.Data.Dtos.Room
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
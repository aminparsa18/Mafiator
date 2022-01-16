using System;
using MessagePack;

namespace Mafiator.Data.Dtos.Room
{
    [MessagePackObject]
    public class RoomIdDto
    {
        [Key(0)] 
        public Guid Id { get; set; }
    }
}
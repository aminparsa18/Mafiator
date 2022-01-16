using MessagePack;
using System;

namespace MafiatorApp.Dtos.Game
{
    [MessagePackObject()]
    public class GameDto
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public short Capacity{ get; set; }
        [Key(2)]
        public Guid RoomId{ get; set; }
        [Key(3)]
        public short Count{ get; set; }
        [Key(4)]
        public DateTime Date{ get; set; }
    }
}

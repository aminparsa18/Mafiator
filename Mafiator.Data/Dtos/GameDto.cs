using System;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class GameDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
        [Key(1)]
        public short Capacity{ get; set; }
        [Key(2)]
        public Ulid RoomId{ get; set; }
        [Key(3)]
        public string RoomImage{ get; set; }
        [Key(4)]
        public short Count { get; set; }
        [Key(5)]
        public DateTime Date{ get; set; }
    }
}

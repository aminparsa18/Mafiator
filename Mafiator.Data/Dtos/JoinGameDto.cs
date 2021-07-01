using System;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class JoinGameDto
    {
        [Key(0)]
        public string ConnectionId { get; set; }
        [Key(1)]
        public Ulid GameId { get; set; }
    }
}

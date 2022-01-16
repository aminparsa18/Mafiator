using MessagePack;
using System;

namespace Mafiator.Data.Dtos.Game
{
    [MessagePackObject()]
    public class JoinGameDto
    {
        [Key(0)]
        public string ConnectionId { get; set; }
        [Key(1)]
        public Guid GameId { get; set; }
    }
}

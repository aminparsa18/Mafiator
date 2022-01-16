using Mafiator.Entities.Enums;
using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Game
{
    [MessagePackObject]
    public class WaitingGameDto
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public DateTime Date { get; set; }
        [Key(2)]
        public List<GameRole> Roles { get; set; }
        [Key(3)]
        public List<WaitingGameMemberDto> Members { get; set; }
        [Key(4)]
        public GameStatus Status{ get; set; }
    }
}

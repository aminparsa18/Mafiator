using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Vote
{
    [MessagePackObject()]
    public class VoteDto
    {
        [Key(0)]
        public Guid GameId { get; set; }
        [Key(1)]
        public Guid VoterId { get; set; }
        [Key(2)]
        public List<Guid> Targets { get; set; }
    }
}

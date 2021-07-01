using System;
using System.Collections.Generic;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class VoteDto
    {
        [Key(0)]
        public Ulid GameId { get; set; }
        [Key(1)]
        public Ulid VoterId { get; set; }
        [Key(2)]
        public List<Ulid> Targets { get; set; }
    }
}

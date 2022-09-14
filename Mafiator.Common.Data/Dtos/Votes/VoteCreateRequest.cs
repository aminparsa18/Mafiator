using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.Votes
{
    /// <summary>
    /// Vote create request dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class VoteCreateRequest
    {
        /// <summary>
        /// Game key identifier.
        /// </summary>
        [Key(0)]
        public Guid GameId { get; set; }

        /// <summary>
        /// Voter key identifier.
        /// </summary>
        [Key(1)]
        public Guid VoterId { get; set; }

        /// <summary>
        /// List of targets.
        /// </summary>
        [Key(2)]
        public List<Guid> Targets { get; set; }
    }
}
using MemoryPack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.Votes;

/// <summary>
/// Vote create request dto.
/// </summary>
[MemoryPackable]
public sealed partial class VoteCreateRequest
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Voter key identifier.
    /// </summary>
    public Guid VoterId { get; set; }

    /// <summary>
    /// List of targets.
    /// </summary>
    public List<Guid> Targets { get; set; }
}
using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Vote;

/// <summary>
/// Vote dto.
/// </summary>
[MessagePackObject()]
public class VoteDto
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
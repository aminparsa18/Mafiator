using MemoryPack;

namespace Mafiator.Common.Data.Dtos.Votes;

/// <summary>
/// Vote details result dto.
/// </summary>
[MemoryPackable]
public sealed partial class VoteDetailsResult
{
    /// <summary>
    /// Voter key identifier.
    /// </summary>
    public string VoterId { get; set; }

    /// <summary>
    /// Target key identifier.
    /// </summary>
    public string TargetId { get; set; }
}
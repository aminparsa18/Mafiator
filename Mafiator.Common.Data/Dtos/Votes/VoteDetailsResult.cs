namespace Mafiator.Common.Data.Dtos.Votes;

/// <summary>
/// Vote details result dto.
/// </summary>
[MessagePackObject()]
public class VoteDetailsResult
{
    /// <summary>
    /// Voter key identifier.
    /// </summary>
    [Key(0)]
    public string VoterId { get; set; }

    /// <summary>
    /// Target key identifier.
    /// </summary>
    [Key(1)]
    public string TargetId { get; set; }
}
namespace Mafiator.Data.Dtos.Vote;

/// <summary>
/// Vote status dto.
/// </summary>
[MessagePackObject()]
public class VoteStatusDto
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
using Mafiator.Game.Enums;

namespace Mafiator.Game.Models.Vote;

public sealed class VoteStatusResultDto
{
    public string MemberId { get; set; }
    public string DisplayName { get; set; }
    public string Image { get; set; }
    public string Votes { get; set; }
    public CandidateStatus Status { get; set; }
    public List<string> Voters { get; set; }
}
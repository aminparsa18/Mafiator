using Mafiator.Common.Data.Enums;

namespace Mafiator.Data.Dtos.User;

/// <summary>
/// User game status dto.
/// </summary>
public class UserGameStatusDto
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    public string MemberId{ get; set; }

    /// <summary>
    /// Game status.
    /// </summary>
    public GameStatus GameStatus { get; set; }

    /// <summary>
    /// Game role.
    /// </summary>
    public GameRole GameRole { get; set; }
}
namespace Mafiator.Data.Dtos.User;

/// <summary>
/// User status dto.
/// </summary>
[MessagePackObject()]
public class UserStatusDto
{
    /// <summary>
    /// Wins count as mafia.
    /// </summary>
    [Key(0)]
    public double MafiaWin { get; set; }

    /// <summary>
    /// Wins count as citizen.
    /// </summary>
    [Key(1)]
    public double CitizenWin { get; set; }

    /// <summary>
    /// Total wins count.
    /// </summary>
    [Key(2)]
    public double TotalWin { get; set; }
}
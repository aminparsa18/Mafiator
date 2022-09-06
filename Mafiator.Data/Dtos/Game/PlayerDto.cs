using Mafiator.Common.Enums;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Player dto.
/// </summary>
[MessagePackObject()]
public class PlayerDto
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    [Key(0)]
    public string MemberId { get; set; }

    /// <summary>
    /// Game member user key identifier.
    /// </summary>
    [Key(1)]
    public string UserId { get; set; }

    /// <summary>
    /// Player game role.
    /// </summary>
    [Key(2)]
    public GameRole Role{ get; set; }

    /// <summary>
    /// Player status.
    /// </summary>
    [Key(3)]
    public PlayerStatus Status{ get; set; }
}
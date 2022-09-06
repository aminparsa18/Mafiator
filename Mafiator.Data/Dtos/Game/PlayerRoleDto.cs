using Mafiator.Common.Data.Enums;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Player role dto.
/// </summary>
[MessagePackObject()]
public class PlayerRoleDto
{
    /// <summary>
    /// Player game role.
    /// </summary>
    [Key(0)]
    public GameRole Role { get; set; }

    /// <summary>
    /// Game member key identifier.
    /// </summary>
    [Key(1)]
    public string MemberId{ get; set; }
}
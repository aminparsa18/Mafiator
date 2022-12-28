using Mafiator.Common.Data.Enums;
using MemoryPack;

namespace Mafiator.Common.Data.Dtos.GameMembers;

/// <summary>
/// Player role dto.
/// </summary>
[MemoryPackable]
public sealed partial class PlayerRoleResult
{
    /// <summary>
    /// Player game role.
    /// </summary>
    public GameRole Role { get; set; }

    /// <summary>
    /// Game member key identifier.
    /// </summary>
    public string MemberId { get; set; }
}
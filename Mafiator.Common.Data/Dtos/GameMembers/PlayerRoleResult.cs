using Mafiator.Common.Data.Enums;
using MessagePack;

namespace Mafiator.Common.Data.Dtos.GameMembers
{
    /// <summary>
    /// Player role dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class PlayerRoleResult
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
        public string MemberId { get; set; }
    }
}
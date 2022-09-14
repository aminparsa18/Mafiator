using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.Games
{
    /// <summary>
    /// Game create dto.
    /// </summary>
    [MessagePackObject]
    public sealed class GameCreateRequest
    {
        /// <summary>
        /// Start date.
        /// </summary>
        [Key(0)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Game roles.
        /// </summary>
        [Key(1)]
        public List<GameRoleCreateRequest> Roles { get; set; }

        /// <summary>
        /// Room key identifier.
        /// </summary>
        [Key(2)]
        public Guid RoomId { get; set; }
    }
}
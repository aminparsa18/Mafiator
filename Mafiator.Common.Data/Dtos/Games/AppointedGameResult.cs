using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.Games
{
    /// <summary>
    /// Waiting game dto.
    /// </summary>
    [MessagePackObject]
    public sealed class AppointedGameResult
    {
        /// <summary>
        /// Game key identifier.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Game start date.
        /// </summary>
        [Key(1)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// List of game roles.
        /// </summary>
        [Key(2)]
        public List<GameRole> Roles { get; set; }

        /// <summary>
        /// List of waiting game members.
        /// </summary>
        [Key(3)]
        public List<WaitingPlayerResult> Members { get; set; }

        /// <summary>
        /// Game status.
        /// </summary>
        [Key(4)]
        public GameStatus Status { get; set; }
    }
}
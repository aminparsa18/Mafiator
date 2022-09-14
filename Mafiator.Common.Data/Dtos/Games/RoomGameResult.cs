using Mafiator.Common.Data.Enums;
using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Games
{
    /// <summary>
    /// Room game dto.
    /// </summary>
    [MessagePackObject]
    public sealed class RoomGameResult
    {
        /// <summary>
        /// Game start date.
        /// </summary>
        [Key(0)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Game status.
        /// </summary>
        [Key(1)]
        public GameStatus Status { get; set; }
    }
}
using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Games
{
    /// <summary>
    /// Game creation result dto.
    /// </summary>
    [MessagePackObject]
    public sealed class GameCreateResult
    {
        /// <summary>
        /// Game key identifier.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; }
    }
}
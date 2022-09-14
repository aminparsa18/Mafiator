using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Gems
{
    /// <summary>
    /// Gem dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class GemResult
    {
        /// <summary>
        /// Gem key identifier.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gem count.
        /// </summary>
        [Key(1)]
        public int Count { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        [Key(2)]
        public int Price { get; set; }

        /// <summary>
        /// Image.
        /// </summary>
        [Key(3)]
        public string Image { get; set; }
    }
}
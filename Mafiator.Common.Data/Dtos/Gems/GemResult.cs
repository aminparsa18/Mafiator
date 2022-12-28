using MemoryPack;
using System;

namespace Mafiator.Common.Data.Dtos.Gems;

/// <summary>
/// Gem dto.
/// </summary>
[MemoryPackable]
public sealed partial class GemResult
{
    /// <summary>
    /// Gem key identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gem count.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Price.
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }
}
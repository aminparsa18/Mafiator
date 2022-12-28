using MemoryPack;
using System;

namespace Mafiator.Common.Data.Dtos.Reactions;

/// <summary>
/// Reaction dto
/// </summary>
[MemoryPackable]
public sealed partial class ReactionResult
{
    /// <summary>
    /// Reaction key Identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }
}
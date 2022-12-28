using MemoryPack;
using System;

namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Game dto.
/// </summary>
[MemoryPackable]
public sealed partial class AvailableGameResult
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Capacity.
    /// </summary>
    public short Capacity { get; set; }

    /// <summary>
    /// Room key identifier.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Game member count.
    /// </summary>
    public short MemberCount { get; set; }

    /// <summary>
    /// Game start date.
    /// </summary>
    public DateTime StartDate { get; set; }
}
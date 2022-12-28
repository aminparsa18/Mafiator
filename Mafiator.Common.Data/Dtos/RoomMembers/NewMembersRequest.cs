using MemoryPack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.RoomMembers;

/// <summary>
/// Add game member dto.
/// </summary>
[MemoryPackable]
public sealed partial class NewMembersRequest
{
    /// <summary>
    /// Room key identifier.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// List of users.
    /// </summary>
    public List<Guid> Users { get; set; }
}
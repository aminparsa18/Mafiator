using MessagePack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.RoomMembers
{
    /// <summary>
    /// Add game member dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class NewMembersRequest
    {
        /// <summary>
        /// Room key identifier.
        /// </summary>
        [Key(0)]
        public Guid RoomId { get; set; }

        /// <summary>
        /// List of users.
        /// </summary>
        [Key(1)]
        public List<Guid> Users { get; set; }
    }
}
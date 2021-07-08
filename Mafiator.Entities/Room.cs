using System;
using System.Collections.Generic;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class Room:BaseEntity
    {
        public string Name { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid UserId { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public bool IsPrivate { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Game> Game { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage{ get; set; }
        public virtual ICollection<RoomMember> RoomMember { get; set; }
    }
}

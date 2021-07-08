using System;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class ChatMessage:BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid RoomId{ get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid UserId { get; set; }
        public GameMessageType MessageType { get; set; }
        public string Content { get; set; }
        public virtual Room Room{ get; set; }
        public virtual User User { get; set; }
    }
}

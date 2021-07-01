using System;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Enums;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class GameEvent:BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid GameId { get; set; }
        public bool IsValidated{ get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid MemberId { get; set; }
        public GameEventType EventType { get; set; }

        public virtual Game Game { get; set; }
        public virtual GameMember Member{ get; set; }
    }
}

using System;
using System.Collections.Generic;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
   public class GameMember:BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid GameId { get; set; }
        [PropertyHandler(typeof(NullableUlidPropertyHandler))]
        public Ulid? UserId { get; set; }
        public GameRole Role { get; set; } 
        public PlayerStatus Status{ get; set; } 
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Vote> Voter{ get; set; }
        public virtual ICollection<Vote> Target{ get; set; }
        public virtual ICollection<GameEvent> GameEvent{ get; set; }
    }
}

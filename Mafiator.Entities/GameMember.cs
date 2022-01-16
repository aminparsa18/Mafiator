using Mafiator.Entities.Enums;
using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities
{
    public class GameMember:BaseEntity
    {
        public Guid GameId { get; set; }
        public Guid? UserId { get; set; }
        public GameRole Role { get; set; } 
        public PlayerStatus Status{ get; set; } 
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Vote> Voter{ get; set; }
        public virtual ICollection<Vote> Target{ get; set; }
        public virtual ICollection<GameEvent> GameEvent{ get; set; }
    }
}

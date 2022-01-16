using System;

namespace Mafiator.Entities
{
    public class Vote:BaseEntity
    {
        public Guid GameId { get; set; }
        public Guid VoterId { get; set; }
        public Guid TargetId { get; set; }
        public bool IsValidated { get; set; }
        public virtual Game Game { get; set; }
        public virtual GameMember Voter { get; set; }
        public virtual GameMember Target { get; set; }
    }
}

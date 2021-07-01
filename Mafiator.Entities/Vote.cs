using System;
using Mafiator.Common.Helpers;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
   public class Vote:BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid GameId { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid VoterId { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid TargetId { get; set; }
        public bool IsValidated { get; set; }
        public virtual Game Game { get; set; }
        public virtual GameMember Voter { get; set; }
        public virtual GameMember Target { get; set; }
    }
}

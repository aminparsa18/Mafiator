using System;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
   public class EventJoin:BaseEntity
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]

        public Ulid EventId { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]

        public Ulid UserId { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}

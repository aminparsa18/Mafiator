using System;
using System.Collections.Generic;
using Mafiator.Common.Helpers;
using Mafiator.Entities.Identity;
using RepoDb.Attributes;

namespace Mafiator.Entities
{
    public class Event:BaseEntity
    {
        public string CityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EventJoin> EventJoin { get; set; }
    }
}

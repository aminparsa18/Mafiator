using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

public class EventJoin:BaseEntity
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public virtual Event Event { get; set; }
    public virtual User User { get; set; }
}

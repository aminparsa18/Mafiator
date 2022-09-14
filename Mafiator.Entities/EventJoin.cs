using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

public sealed class EventJoin : BaseEntity
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Event Event { get; set; }
    public User User { get; set; }
}
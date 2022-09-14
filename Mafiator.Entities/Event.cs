using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities;

public sealed class Event : BaseEntity
{
    public string CityId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<EventJoin> EventJoin { get; set; }
}
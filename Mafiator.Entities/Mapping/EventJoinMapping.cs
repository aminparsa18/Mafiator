using Mafiator.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping;

/// <summary>
/// Table mapping for event joins.
/// </summary>
public class EventJoinMapping : BaseEntityTypeConfiguration<EventJoin>
{
    public override void Configure(EntityTypeBuilder<EventJoin> builder)
    {
        builder.HasOne(d => d.Event)
            .WithMany(p => p.EventJoin)
            .HasForeignKey(d => d.EventId)
            .HasConstraintName("FK_EventJoin_Event");

        builder.HasOne(d => d.User)
            .WithMany(p => p.EventJoin)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_EventJoin_User");

        base.Configure(builder);
    }
}
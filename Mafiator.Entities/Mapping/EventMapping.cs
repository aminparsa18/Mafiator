using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping;

/// <summary>
/// Table mapping for events.
/// </summary>
public class EventMapping : BaseEntityTypeConfiguration<Event>
{
    public override void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();
        base.Configure(builder);
    }
}
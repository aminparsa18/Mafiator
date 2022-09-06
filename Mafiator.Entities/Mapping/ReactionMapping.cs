using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping;

/// <summary>
/// Table mapping for reaction.
/// </summary>
public class ReactionMapping : BaseEntityTypeConfiguration<Reaction>
{
    public override void Configure(EntityTypeBuilder<Reaction> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Title).IsRequired();
        builder.Property(e => e.Image).IsRequired();
    }
}
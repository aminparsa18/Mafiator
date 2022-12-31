using Mafiator.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping;

/// <summary>
/// Table mapping for event countries.
/// </summary>
public class CountryMapping : BaseEntityTypeConfiguration<Country>
{
    public override void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(5).IsRequired();
        builder.Property(e => e.Sign).HasMaxLength(2).IsRequired();

        base.Configure(builder);
    }
}
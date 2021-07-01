using Mafiator.Entities.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class EventMapping: BaseEntityTypeConfiguration<Event>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(p => p.UserId).HasConversion(new UlidToStringConverter());
            builder.Property(p => p.Title).HasMaxLength(150).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();
            base.Configure(builder);
        }
    }
}

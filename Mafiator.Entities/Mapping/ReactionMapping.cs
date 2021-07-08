using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
   public class ReactionMapping:BaseEntityTypeConfiguration<Reaction>
    {
        public override void Configure(EntityTypeBuilder<Reaction> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.Image).IsRequired();
        }
    }
}

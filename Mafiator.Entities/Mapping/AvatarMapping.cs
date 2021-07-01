using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
   public class AvatarMapping:BaseEntityTypeConfiguration<Avatar>
    {
        public override void Configure(EntityTypeBuilder<Avatar> builder)
        {
            builder.Property(p =>p.Name).IsRequired();
            base.Configure(builder);
        }
    }
}

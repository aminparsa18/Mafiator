using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class BaseMapping:IEntityTypeConfiguration<BaseEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntity> builder)
        {
            builder.Property(p => p.CreatedDate).HasColumnType("datetime2").HasDefaultValueSql("getdate()");
            builder.Property(p => p.ModifiedDate).HasColumnType("datetime2").HasDefaultValueSql("getdate()");
        }
    }
}

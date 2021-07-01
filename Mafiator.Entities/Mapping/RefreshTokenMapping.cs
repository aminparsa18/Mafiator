using Mafiator.Entities.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class RefreshTokenMapping:BaseEntityTypeConfiguration<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(p => p.UserId).HasConversion(new UlidToStringConverter());
            builder.Property(p => p.ExpirationDate).HasColumnType("datetime2");
            builder.Property(p => p.Token).IsRequired();
            builder.Property(p => p.JwtId).IsRequired();
            base.Configure(builder);
        }
    }
}

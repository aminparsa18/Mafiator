using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class RoomMapping:BaseEntityTypeConfiguration<Room>
    {
        public override void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(e => e.Code).IsRequired();
            builder.HasOne(d => d.User)
                .WithMany(p => p.Room)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Room_User");
            base.Configure(builder);
            builder.HasIndex(e => e.Country);
        }
    }
}

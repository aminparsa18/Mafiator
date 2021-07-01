using Mafiator.Entities.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
   public class EventJoinMapping:BaseEntityTypeConfiguration<EventJoin>
    {
        public override void Configure(EntityTypeBuilder<EventJoin> builder)
        {
            builder.Property(p => p.UserId).HasConversion(new UlidToStringConverter());
            builder.Property(p => p.EventId).HasConversion(new UlidToStringConverter());
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
}

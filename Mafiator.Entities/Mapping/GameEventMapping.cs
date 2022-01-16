using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class GameEventMapping:BaseEntityTypeConfiguration<GameEvent>
    {
        public override void Configure(EntityTypeBuilder<GameEvent> builder)
        {
            builder.Property(p => p.EventType).IsRequired().HasColumnType("smallint");

            builder.HasOne(d => d.Game)
                .WithMany(p => p.GameEvent)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameEvent_Game");

            builder.HasOne(d => d.Member)
                .WithMany(p => p.GameEvent)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_GameEvent_Member");
            base.Configure(builder);
        }
    }
}

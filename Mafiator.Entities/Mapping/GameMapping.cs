using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping;

/// <summary>
/// Table mapping for game.
/// </summary>
public class GameMapping : BaseEntityTypeConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(p => p.StartDate).HasColumnType("datetime2");
        builder.Property(p => p.Status).IsRequired().HasColumnType("smallint").HasDefaultValueSql("((0))");
        builder.HasOne(d => d.Room)
            .WithMany(p => p.Game)
            .HasForeignKey(d => d.RoomId)
            .HasConstraintName("FK_Room_Game");
        builder.HasIndex(b => b.RoomId);

        base.Configure(builder);
    }
}
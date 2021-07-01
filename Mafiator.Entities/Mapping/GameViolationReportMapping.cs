using Mafiator.Entities.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class GameViolationReportMapping:BaseEntityTypeConfiguration<GameViolationReport>
    {
        public override void Configure(EntityTypeBuilder<GameViolationReport> builder)
        {
            builder.Property(p => p.ReportedId).HasConversion(new UlidToStringConverter());
            builder.Property(p => p.ReporterId).HasConversion(new UlidToStringConverter());

            builder.Property(p => p.GameId).HasConversion(new UlidToStringConverter());
            builder.Property(p => p.ViolationType).HasColumnType("smallint");

            builder.HasOne(d => d.Reporter)
                .WithMany(p => p.Reporter)
                .HasForeignKey(d => d.ReporterId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_GameViolationReport_Reporter");

            builder.HasOne(d => d.Reported)
                .WithMany(p => p.Reported)
                .HasForeignKey(d => d.ReportedId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_GameViolationReport_Reported");
            builder.HasOne(d => d.Game)
                .WithMany(p => p.Report)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameViolationReport_Game");

            base.Configure(builder);
        }
    }
}

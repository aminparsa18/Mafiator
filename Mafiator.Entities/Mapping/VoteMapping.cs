using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping
{
    public class VoteMapping : BaseEntityTypeConfiguration<Vote>
    {
        public override void Configure(EntityTypeBuilder<Vote> builder)
        {

            builder.HasOne(d => d.Game)
                .WithMany(p => p.Vote)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Vote_Game")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Voter)
                .WithMany(p => p.Voter)
                .HasForeignKey(d => d.VoterId)
                .HasConstraintName("FK_Voter_Voter")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Target)
                .WithMany(p => p.Target)
                .HasForeignKey(d => d.TargetId)
                .HasConstraintName("FK_Target_Target")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(i => i.GameId);
            builder.HasIndex(i => i.VoterId);
            builder.HasIndex(i => i.TargetId);
            base.Configure(builder);
        }
    }
}
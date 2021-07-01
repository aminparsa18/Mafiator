using Microsoft.EntityFrameworkCore;

namespace Mafiator.Entities.Mapping
{
    public static class AllMapping
    {
        public static void AddCustomMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AvatarMapping());
            modelBuilder.ApplyConfiguration(new EventJoinMapping());
            modelBuilder.ApplyConfiguration(new EventMapping());
            modelBuilder.ApplyConfiguration(new GameEventMapping());
            modelBuilder.ApplyConfiguration(new GameMapping());
            modelBuilder.ApplyConfiguration(new GameMemberMapping());
            modelBuilder.ApplyConfiguration(new GameMessageMapping());
            modelBuilder.ApplyConfiguration(new GameViolationReportMapping());
            modelBuilder.ApplyConfiguration(new GemMapping());
            modelBuilder.ApplyConfiguration(new RefreshTokenMapping());
            modelBuilder.ApplyConfiguration(new RoomMapping());
            modelBuilder.ApplyConfiguration(new RoomMemberMapping());
            modelBuilder.ApplyConfiguration(new VoteMapping());
        }
    }
}

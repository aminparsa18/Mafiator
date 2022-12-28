using Mafiator.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafiator.Entities.Mapping;

/// <summary>
/// Table mapping for chat messages.
/// </summary>
public class ChatMessageMapping : BaseEntityTypeConfiguration<ChatMessage>
{
    public override void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.Property(p => p.MessageType).HasColumnType("smallint");
        builder.Property(p => p.Content).IsRequired().HasMaxLength(500);

        builder.HasOne(d => d.Room)
            .WithMany(p => p.ChatMessage)
            .HasForeignKey(d => d.RoomId)
            .HasConstraintName("FK_ChatMessage_Room");

        builder.HasOne(d => d.User)
            .WithMany(p => p.ChatMessage)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_ChatMessage_User");

        builder.HasIndex(i => i.RoomId);

        base.Configure(builder);
    }
}
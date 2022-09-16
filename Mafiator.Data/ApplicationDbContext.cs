using Mafiator.Entities;
using Mafiator.Entities.Extensions;
using Mafiator.Entities.Identity;
using Mafiator.Entities.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mafiator.Data;

public class ApplicationDbContext:IdentityDbContext<User, Role, Guid, UserClaim, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Avatar> Avatar{ get; set; }
    public virtual DbSet<Event> Event{ get; set; }
    public virtual DbSet<EventJoin> EventJoin{ get; set; }
    public virtual DbSet<ChatMessage> ChatMessage{ get; set; }
    public virtual DbSet<Game> Game{ get; set; }
    public virtual DbSet<GameEvent> GameEvent{ get; set; }
    public virtual DbSet<GameMember> GameMember{ get; set; }
    public virtual DbSet<GameMessage> GameMessage{ get; set; }
    public virtual DbSet<GameViolationReport> GameViolationReport{ get; set; }
    public virtual DbSet<Gem> Gem{ get; set; }
    public virtual DbSet<Reaction> Reaction{ get; set; }
    public virtual DbSet<RefreshToken> RefreshToken{ get; set; }
    public virtual DbSet<Room> Room{ get; set; }
    public virtual DbSet<RoomMember> RoomMember{ get; set; }
  
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("dbo");
        builder.ApplyConfigurationsFromAssembly(typeof(AvatarMapping).Assembly);
        builder.AddCustomIdentityMappings();
    }
}
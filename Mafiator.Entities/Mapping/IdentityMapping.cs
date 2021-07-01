using Mafiator.Entities.Converters;
using Mafiator.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mafiator.Entities.Mapping
{
    public static class IdentityMapping
    {
        public static void AddCustomIdentityMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasIndex(e => e.Code).IsUnique();
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaim");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<User>().Property(e => e.Id).HasConversion(new UlidToStringConverter());
            modelBuilder.Entity<Role>().Property(e => e.Id).HasConversion(new UlidToStringConverter());
            modelBuilder.Entity<User>().Property(e => e.Code).IsRequired();
            modelBuilder.Entity<User>().HasIndex(e => e.CountryCode);
           // modelBuilder.Entity<RoleClaim>().Property(e => e.Id).HasConversion(new UlidToStringConverter());
           // modelBuilder.Entity<UserClaim>().Property(e => e.Id).HasConversion(new UlidToStringConverter());
            modelBuilder.Entity<UserRole>()
                .HasOne(userRole => userRole.Role)
                .WithMany(role => role.Users).HasForeignKey(r => r.RoleId);

            modelBuilder.Entity<UserRole>()
               .HasOne(userRole => userRole.User)
               .WithMany(role => role.Roles).HasForeignKey(r => r.UserId);

            modelBuilder.Entity<RoleClaim>()
                 .HasOne(roleclaim => roleclaim.Role)
                 .WithMany(claim => claim.Claims).HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<UserClaim>()
                .HasOne(userClaim => userClaim.User)
                .WithMany(claim => claim.Claims).HasForeignKey(c => c.UserId);
        }
    }

}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationWithIdentity.Models;

namespace WebApplicationWithIdentity
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUsers");
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.displayName).HasMaxLength(200);
            });
            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.ToTable("AppRoles");
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AppUserRoles");
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AppUserClaims");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AppUserLogins");
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AppRoleClaims");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AppUserTokens");
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });
            
          


        }




    }
}

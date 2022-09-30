using B2C2_Pi4_PFFF_Gaetano.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace B2C2_Pi4_PFFF_Gaetano.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<CameraLocation> CameraLocations { get; set; }
        public DbSet<CameraReport> CameraReports { get; set; }

        /* public DbSet<AppUserAchievement> AppUserAchievements { get; set; }
           Disable in order to query AppUserAchievement data directly. */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            modelBuilder.Entity<AppUserAchievement>().HasKey(aua => new { aua.AppUserId, aua.AchievementId });

            modelBuilder.Entity<AppUserAchievement>()
            .HasOne(aua => aua.AppUser)
            .WithMany(au => au.AppUserAchievements)
            .HasForeignKey(aua => aua.AppUserId);

            modelBuilder.Entity<AppUserAchievement>()
            .HasOne(aua => aua.Achievement)
            .WithMany(a => a.AppUserAchievements)
            .HasForeignKey(aua => aua.AchievementId);

            modelBuilder.Entity<AppUser>()
            .HasMany(au => au.CameraReports)
            .WithOne(cr => cr.AppUser)
            .HasForeignKey(au => au.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Camera>()
            .HasMany(c => c.CameraReports)
            .WithOne(cr => cr.Camera)
            .HasForeignKey(cr => cr.CameraId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CameraLocation>()
            .HasMany(cl => cl.Cameras)
            .WithOne(c => c.CameraLocation)
            .HasForeignKey(c => c.CameraLocationId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
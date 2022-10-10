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

        /* public DbSet<AppUserAchievement> AppUserAchievements { get; set; }
           Disable in order to query AppUserAchievement data directly. */

        public DbSet<B2C2_Pi4_PFFF_Gaetano.Models.AppUserAchievement> AppUserAchievement { get; set; }
    }
}
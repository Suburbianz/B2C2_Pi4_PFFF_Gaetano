using B2C2_Pi4_PFFF_Gaetano.Models;
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
        // public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<CameraInformation> CameraInformations { get; set; }
        public DbSet<CameraLocation> CameraLocations { get; set; }
        public DbSet<CameraReport> CameraReports { get; set; }

    }
}